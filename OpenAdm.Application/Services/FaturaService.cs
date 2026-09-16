using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.FaturasModel;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

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
            return (ResultPartner<ResultadoPadraoViewModel>)
                "A cobrança do pedido não está disponível para faturamento!";
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
            tipo: TipoFaturaEnum.AReceber,
            total: cobranca.Total);

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
        parcela.Transacoes ??= [];
        parcela.Transacoes.Add(parcela.Pagar(
            valor: cobranca.Total - (dto.Desconto ?? 0),
            meioDePagamento: MeioDePagamentoEnum.Dinheiro,
            observacao: "Baixa automática da cobrança do pedido",
            dataDePagamento: data,
            desconto: dto.Desconto,
            juros: null));

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
            tipo: TipoFaturaEnum.AReceber,
            total: cobranca.Total);

        foreach (var parcelaDto in dto.Parcelas)
        {
            var valorParcela = parcelaDto.Valor.ArredondarCentavos();
            var parcela = new Parcela(
                id: Guid.NewGuid(),
                dataDeCriacao: data,
                dataDeAtualizacao: data,
                numero: 0,
                dataDeVencimento: parcelaDto.DataDeVencimento,
                numeroDaParcela: parcelaDto.NumeroDaParcela,
                meioDePagamento: parcelaDto.MeioDePagamento,
                valor: valorParcela,
                observacao: null,
                faturaId: fatura.Id,
                idExterno: null,
                desconto: null,
                tipo: TipoFaturaEnum.AReceber,
                quitada: false,
                juros: null);

            parcela.Fatura = fatura;

            if (parcelaDto.AVista)
            {
                parcela.Pagar(
                    valor: valorParcela,
                    meioDePagamento: parcelaDto.MeioDePagamento,
                    observacao: "Pagamento à vista na negociação da cobrança",
                    dataDePagamento: data,
                    desconto: null,
                    juros: null);
            }

            fatura.Parcelas.Add(parcela);
        }

        if (fatura.Parcelas.All(x => x.Quitada))
        {
            fatura.Fechar();
        }
        else if (fatura.Parcelas.Any(x => x.Quitada))
        {
            fatura.PagaParcialmente();
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

    public async Task<ResultPartner<ResultadoPadraoViewModel>> RenegociarAsync(RenegociarFaturaDto dto)
    {
        var erro = dto.Validar();
        if (erro != null)
            return (ResultPartner<ResultadoPadraoViewModel>)erro;

        var fatura = await _contasAReceberRepository.ObterParaRenegociarAsync(dto.FaturaId);
        if (fatura == null)
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar a fatura!";

        if (fatura.Tipo == TipoFaturaEnum.Bonificado)
            return (ResultPartner<ResultadoPadraoViewModel>)"Não é possível renegociar uma fatura bonificada!";

        var parcelasAtivas = fatura.Parcelas.Where(x => x.Ativo).ToList();
        var parcelasParaCriar = dto.Parcelas.ToList();
        var parcelasPreservadas = new List<Parcela>();
        var parcelasParaInativar = new List<Parcela>();
        var baixasParciaisParaConsolidar = new List<Parcela>();

        foreach (var parcela in parcelasAtivas)
        {
            if (parcela.Quitada)
            {
                parcelasPreservadas.Add(parcela);
                continue;
            }

            if (parcela.ValorPagoRecebido > 0)
            {
                parcelasPreservadas.Add(parcela);
                if (parcela.Valor > parcela.ValorPagoRecebido)
                    baixasParciaisParaConsolidar.Add(parcela);
                continue;
            }

            var mesmaParcela = parcelasParaCriar.FirstOrDefault(x =>
                x.NumeroDaParcela == parcela.NumeroDaParcela
                && x.Valor.ArredondarCentavos() == parcela.Valor
                && x.DataDeVencimento == parcela.DataDeVencimento
                && x.MeioDePagamento == parcela.MeioDePagamento);

            if (mesmaParcela != null)
            {
                parcelasPreservadas.Add(parcela);
                parcelasParaCriar.Remove(mesmaParcela);
                continue;
            }

            if (!string.IsNullOrWhiteSpace(parcela.IdExterno))
                return (ResultPartner<ResultadoPadraoViewModel>)
                    $"A parcela {parcela.NumeroDaParcela} possui integração externa e não pode ser alterada!";

            parcelasParaInativar.Add(parcela);
        }

        var numerosPreservados = parcelasPreservadas.Select(x => x.NumeroDaParcela).ToList();
        if (numerosPreservados.Distinct().Count() != numerosPreservados.Count)
            return (ResultPartner<ResultadoPadraoViewModel>)
                "A fatura já possui números repetidos entre parcelas ativas preservadas!";

        var totalPreservado = parcelasPreservadas.Sum(x =>
            x.ValorPagoRecebido > 0
                ? x.ValorPagoRecebido.ArredondarCentavos()
                : x.Valor);
        erro = dto.ValidarTotal(fatura.Total - totalPreservado, parcelasParaCriar);
        if (erro != null)
            return (ResultPartner<ResultadoPadraoViewModel>)erro;

        foreach (var parcela in baixasParciaisParaConsolidar)
            parcela.ConsolidarBaixaParcial();

        foreach (var parcela in parcelasParaInativar)
            parcela.Inativar();

        var numerosOcupados = numerosPreservados.ToHashSet();
        var proximoNumero = 1;
        var novasParcelas = new List<Parcela>();
        foreach (var informada in parcelasParaCriar)
        {
            var numeroDaParcela = informada.NumeroDaParcela;
            if (!numerosOcupados.Add(numeroDaParcela))
            {
                while (numerosOcupados.Contains(proximoNumero))
                    proximoNumero++;
                numeroDaParcela = proximoNumero;
                numerosOcupados.Add(numeroDaParcela);
            }

            var nova = Parcela.NovaFatura(
                dataDeVencimento: informada.DataDeVencimento,
                numeroDaParcela: numeroDaParcela,
                meioDePagamento: informada.MeioDePagamento,
                valor: informada.Valor.ArredondarCentavos(),
                observacao: null,
                faturaId: fatura.Id,
                idExterno: null,
                desconto: null,
                juros: null,
                tipoFatura: fatura.Tipo);
            novasParcelas.Add(nova);
        }

        if (novasParcelas.Count > 0)
            await _contasAReceberRepository.AdicionarParcelasAsync(novasParcelas);

        if (novasParcelas.Count > 0 || parcelasParaInativar.Count > 0 || baixasParciaisParaConsolidar.Count > 0)
        {
            var descricao = $"Fatura renegociada: {novasParcelas.Count} parcela(s) criada(s), " +
                            $"{parcelasParaInativar.Count} parcela(s) inativada(s) e " +
                            $"{baixasParciaisParaConsolidar.Count} baixa(s) parcial(is) consolidada(s).";
            await _contasAReceberRepository.AddHistoricoAsync(FaturaHistorico.Nova(fatura.Id, descricao));
            await _contasAReceberRepository.SaveChangesAsync();
        }

        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel { Resultado = true };
    }

    public async Task<ResultPartner<FaturaViewModel>> SugerirParcelamentoAsync(Guid faturaId)
    {
        if (faturaId == Guid.Empty)
            return (ResultPartner<FaturaViewModel>)"Informe a fatura!";

        var fatura = await _contasAReceberRepository.GetByIdCompletaAsync(faturaId);
        if (fatura == null)
            return (ResultPartner<FaturaViewModel>)"Não foi possível localizar a fatura!";

        var parcelasSugeridas = fatura.Parcelas
            .Where(x => x.Ativo && x.ValorAPagarAReceber > 0)
            .Select(ParcelaViewModel.SemRelacionamentos)
            .ToList();
        var sugestao = FaturaViewModel.SemParcelas(fatura);
        sugestao.Parcelas = parcelasSugeridas;

        return (ResultPartner<FaturaViewModel>)sugestao;
    }

    public async Task<FaturaViewModel> CriarAdmAsync(FaturaCriarAdmDto faturaCriarAdmDto)
    {
        faturaCriarAdmDto.Validar();
        _ = await _usuarioService.GetUsuarioByIdAdmAsync(id: faturaCriarAdmDto.UsuarioId);

        var data = DateTime.UtcNow;
        var fatura = new Fatura(
            id: Guid.NewGuid(),
            dataDeCriacao: data,
            dataDeAtualizacao: data,
            numero: 0,
            status: StatusFaturaEnum.Aberta,
            usuarioId: faturaCriarAdmDto.UsuarioId,
            pedidoId: faturaCriarAdmDto.PedidoId,
            dataDeFechamento: null,
            tipo: faturaCriarAdmDto.Tipo,
            total: faturaCriarAdmDto.Parcelas.Sum(x => x.Valor));

        foreach (var parcelaDto in faturaCriarAdmDto.Parcelas)
        {
            var parcela = Parcela.NovaFatura(
                dataDeVencimento: parcelaDto.DataDeVencimento,
                numeroDaParcela: parcelaDto.NumeroDaParcela,
                meioDePagamento: parcelaDto.MeioDePagamento,
                valor: parcelaDto.Valor,
                observacao: parcelaDto.Observacao,
                faturaId: fatura.Id,
                idExterno: null,
                desconto: null,
                juros: null,
                tipoFatura: fatura.Tipo);

            parcela.Fatura = fatura;

            if (parcelaDto.AVista)
            {
                parcela.Transacoes =
                [
                    parcela.Pagar(
                        valor: parcelaDto.Valor,
                        meioDePagamento: parcelaDto.MeioDePagamento,
                        observacao: parcelaDto.Observacao,
                        dataDePagamento: data,
                        desconto: null,
                        juros: null)
                ];
            }

            fatura.Parcelas.Add(parcela);
        }

        if (fatura.Parcelas.All(x => x.Quitada))
        {
            fatura.Fechar();
        }
        else if (fatura.Parcelas.Any(x => x.Quitada))
        {
            fatura.PagaParcialmente();
        }

        await _contasAReceberRepository.AddAsync(fatura);

        return (FaturaViewModel)fatura;
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> CriarBonificadaAsync(BaixaAutomaticaDto dto)
    {
        var pedido = await _pedidoRepository.ObterPedidoParaCobrancaAsync(dto.PedidoId);

        if (pedido == null)
        {
            return (ResultPartner<ResultadoPadraoViewModel>)"Não foi possível localizar o pedido!";
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
            status: StatusFaturaEnum.Paga,
            usuarioId: pedido.UsuarioId,
            pedidoId: pedido.Id,
            dataDeFechamento: data,
            tipo: TipoFaturaEnum.Bonificado,
            total: pedido.ValorTotalCobrar);

        await _contasAReceberRepository.AddAsync(fatura);

        return (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel
        {
            Resultado = true
        };
    }

    public async Task<PaginacaoViewModel<FaturaBonificadaPaginacaoViewModel>> PaginacaoBonificadasAsync(
        FilterModel<Fatura> dto)
    {
        var paginacao = await _contasAReceberRepository.PaginacaoAsync(dto);

        return new PaginacaoViewModel<FaturaBonificadaPaginacaoViewModel>
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values
                .Select(x => (FaturaBonificadaPaginacaoViewModel)x)
                .ToList()
        };
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