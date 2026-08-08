using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public sealed class FaturaService : IFaturaService
{
    private readonly IFaturaRepository _contasAReceberRepository;
    private readonly IUsuarioService _usuarioService;
    private readonly ICobrancaPedidoEcommerceRepository _cobrancaPedidoRepository;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;

    public FaturaService(
        IFaturaRepository contasAReceberRepository,
        IUsuarioService usuarioService,
        ICobrancaPedidoEcommerceRepository cobrancaPedidoRepository,
        IPedidoRepository pedidoRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        _contasAReceberRepository = contasAReceberRepository;
        _usuarioService = usuarioService;
        _cobrancaPedidoRepository = cobrancaPedidoRepository;
        _pedidoRepository = pedidoRepository;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> BaixaAutomaticaAsync(BaixaAutomaticaDto dto)
    {
        var cobranca = await _cobrancaPedidoRepository.GetByPedidoIdAsync(dto.PedidoId, _parceiroAutenticado.Id);

        if (cobranca == null)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar a cobrança do pedido!";
        }

        if (!cobranca.Ativo || cobranca.Status != StatusCobrancaPedidoEcommerceEnum.ACobrar)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"A cobrança do pedido não está disponível para faturamento!";
        }

        var pedido = await _pedidoRepository.ObterPedidoParaCobrancaAsync(cobranca.PedidoId);

        if (pedido == null)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar o pedido da cobrança!";
        }

        var data = DateTime.UtcNow;
        var fatura = new Fatura(
            id: Guid.NewGuid(),
            dataDeCriacao: data,
            dataDeAtualizacao: data,
            numero: 0,
            status: StatusFaturaEnum.Paga,
            usuarioId: pedido.UsuarioId,
            pedidoId: pedido.Id,
            dataDeFechamento: data,
            tipo: TipoFaturaEnum.AReceber);

        var parcela = Parcela.NovaFatura(
            dataDeVencimento: data,
            numeroDaParcela: 1,
            meioDePagamento: MeioDePagamentoEnum.Dinheiro,
            valor: cobranca.Total,
            observacao: null,
            faturaId: fatura.Id,
            idExterno: null,
            desconto: null,
            juros: null,
            tipoFatura: TipoFaturaEnum.AReceber);

        parcela.Fatura = fatura;
        parcela.Pagar(
            valor: cobranca.Total,
            meioDePagamento: MeioDePagamentoEnum.Dinheiro,
            observacao: "Baixa automática da cobrança do pedido",
            dataDePagamento: data,
            desconto: null,
            juros: null);
        
        fatura.Parcelas.Add(parcela);

        await _contasAReceberRepository.AdicionarAsync(fatura);
        await _contasAReceberRepository.SaveChangesAsync();

        await _cobrancaPedidoRepository.AtualizarStatusAsync(
            cobranca.Id,
            _parceiroAutenticado.Id,
            StatusCobrancaPedidoEcommerceEnum.GeradoFatura);

        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel
        {
            Resultado = true
        };
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> NegociarCobrancaAsync(NegociarCobrancaPedidoDto dto)
    {
        var cobranca = await _cobrancaPedidoRepository.GetByPedidoIdAsync(dto.PedidoId, _parceiroAutenticado.Id);

        if (cobranca == null)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar a cobrança do pedido!";
        }

        if (!cobranca.Ativo || cobranca.Status != StatusCobrancaPedidoEcommerceEnum.ACobrar)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"A cobrança do pedido já foi negociada!";
        }

        var erroValidacao = dto.Validar(cobranca.Total);
        if (!string.IsNullOrWhiteSpace(erroValidacao))
        {
            return (ResultPartner<ResultadoPadraoViewModel>)erroValidacao;
        }

        var pedido = await _pedidoRepository.ObterPedidoParaCobrancaAsync(dto.PedidoId);

        if (pedido == null)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar o pedido da cobrança!";
        }

        if (pedido.Fatura != null)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"O pedido já possui uma fatura!";
        }

        var data = DateTime.UtcNow;
        var fatura = new Fatura(
            id: Guid.NewGuid(),
            dataDeCriacao: data,
            dataDeAtualizacao: data,
            numero: 0,
            status: StatusFaturaEnum.Aberta,
            usuarioId: pedido.UsuarioId,
            pedidoId: pedido.Id,
            dataDeFechamento: null,
            tipo: TipoFaturaEnum.AReceber);

        foreach (var parcelaDto in dto.Parcelas)
        {
            fatura.Parcelas.Add(new Parcela(
                id: Guid.NewGuid(),
                dataDeCriacao: data,
                dataDeAtualizacao: data,
                numero: 0,
                dataDeVencimento: parcelaDto.DataDeVencimento,
                numeroDaParcela: parcelaDto.NumeroDaParcela,
                meioDePagamento: parcelaDto.MeioDePagamento,
                valor: decimal.Round(parcelaDto.Valor, 2, MidpointRounding.AwayFromZero),
                observacao: null,
                faturaId: fatura.Id,
                idExterno: null,
                desconto: null,
                tipo: TipoFaturaEnum.AReceber,
                quitada: false,
                juros: null));
        }

        await _contasAReceberRepository.AddAsync(fatura);

        await _cobrancaPedidoRepository.AtualizarStatusAsync(
            cobranca.Id,
            _parceiroAutenticado.Id,
            StatusCobrancaPedidoEcommerceEnum.GeradoFatura);

        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel
        {
            Resultado = true
        };
    }

    public async Task<FaturaViewModel> CriarAdmAsync(FaturaCriarAdmDto faturaCriarAdmDto)
    {
        _ = await _usuarioService.GetUsuarioByIdAdmAsync(id: faturaCriarAdmDto.UsuarioId);

        var fatura = new Fatura(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            status: StatusFaturaEnum.Aberta,
            usuarioId: faturaCriarAdmDto.UsuarioId,
            pedidoId: faturaCriarAdmDto.PedidoId,
            dataDeFechamento: null,
            tipo: faturaCriarAdmDto.Tipo);

        foreach (var parcelaDto in faturaCriarAdmDto.Parcelas)
        {
            //TODO: ajustar criar fatura pelo admin
            // fatura.Parcelas.Add(new Parcela(
            //     id: Guid.NewGuid(),
            //     dataDeCriacao: DateTime.Now,
            //     dataDeAtualizacao: DateTime.Now,
            //     numero: 0,
            //     dataDeVencimento: parcelaDto.DataDeVencimento,
            //     numeroDaParcela: parcelaDto.NumeroDaParcela,
            //     meioDePagamento: parcelaDto.MeioDePagamento,
            //     valor: parcelaDto.Valor,
            //     desconto: parcelaDto.Desconto,
            //     observacao: parcelaDto.Observacao,
            //     faturaId: fatura.Id,
            //     idExterno: null));
        }

        await _contasAReceberRepository.AddAsync(fatura);

        return (FaturaViewModel)fatura;
    }

    public async Task CriarContasAReceberAsync(CriarFaturaDto contasAReceberDto)
    {
        //TODO:
        // var fatura = Fatura.NovaContasAReceber(
        //     usuarioId: contasAReceberDto.UsuarioId,
        //     pedidoId: contasAReceberDto.PedidoId,
        //     total: contasAReceberDto.Total,
        //     quantidadeDeParcelas: contasAReceberDto.QuantidadeDeParcelas,
        //     primeiroVencimento: contasAReceberDto.DataDoPrimeiroVencimento,
        //     meioDePagamento: contasAReceberDto.MeioDePagamento,
        //     desconto: contasAReceberDto.Desconto,
        //     observacao: contasAReceberDto.Observacao,
        //     idExterno: null,
        //     tipo: contasAReceberDto.Tipo);

        //await _contasAReceberRepository.AddAsync(fatura);
    }

    public async Task<FaturaViewModel> GetByIdAsync(Guid id)
    {
        var fatura = await _contasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");
        return (FaturaViewModel)fatura;
    }

    public async Task<FaturaViewModel> GetCompletaAsync(Guid id)
    {
        var fatura = await _contasAReceberRepository.GetByIdCompletaAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");

        return (FaturaViewModel)fatura;
    }

    public async Task VerificarFechamentoAsync(Guid id)
    {
        var contasAReceber = await _contasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a contas a pagar");

        if (contasAReceber
            .Parcelas.Count() == contasAReceber.Parcelas.Count)
        {
            contasAReceber.Fechar();
            contasAReceber.Parcelas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
            return;
        }

        if (contasAReceber
            .Parcelas
            .Count() > 1)
        {
            contasAReceber.PagaParcialmente();
            contasAReceber.Parcelas = [];
            await _contasAReceberRepository.UpdateAsync(contasAReceber);
        }
    }
}
