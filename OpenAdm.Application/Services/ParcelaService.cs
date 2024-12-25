using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.FaturasModel;
using OpenAdm.Application.Models.ParcelasModel;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public sealed class ParcelaService : IParcelaService
{
    private readonly IParcelaRepository _faturaContasAReceberRepository;
    private readonly IFaturaService _contasAReceberService;
    private readonly IUpdateStatusPedidoService _updateStatusPedidoService;
    private readonly IPedidoService _pedidoService;
    private readonly ITransacaoFinanceiraRepository _transacaoFinanceiraRepository;
    public ParcelaService(
        IParcelaRepository faturaContasAReceberRepository,
        IFaturaService contasAReceberService,
        IUpdateStatusPedidoService updateStatusPedidoService,
        IPedidoService pedidoService,
        ITransacaoFinanceiraRepository transacaoFinanceiraRepository)
    {
        _faturaContasAReceberRepository = faturaContasAReceberRepository;
        _contasAReceberService = contasAReceberService;
        _updateStatusPedidoService = updateStatusPedidoService;
        _pedidoService = pedidoService;
        _transacaoFinanceiraRepository = transacaoFinanceiraRepository;
    }

    public async Task<ParcelaViewModel> AddAsync(ParcelaCriarDto parcelaCriarDto)
    {
        parcelaCriarDto.Validar();
        var fatura = await _contasAReceberService.GetByIdAsync(parcelaCriarDto.FaturaId);
        var proximoNumeroParcela = (fatura.Parcelas.MaxBy(x => x.NumeroDaParcela)?.NumeroDaParcela ?? 0) + 1;

        var parcela = Parcela.NovaFatura(
            dataDeVencimento: parcelaCriarDto.DataDeVencimento ?? DateTime.Now,
            numeroDaParcela: proximoNumeroParcela,
            meioDePagamento: parcelaCriarDto.MeioDePagamento,
            valor: parcelaCriarDto.Valor,
            desconto: parcelaCriarDto.Desconto,
            observacao: parcelaCriarDto.Observacao,
            faturaId: parcelaCriarDto.FaturaId,
            idExterno: null);

        await _faturaContasAReceberRepository.AddAsync(parcela);

        return (ParcelaViewModel)parcela;
    }

    public async Task BaixarFaturaWebHookAsync(NotificationFaturaWebHook notificationFaturaWebHook)
    {
        var fatura = await _faturaContasAReceberRepository.GetByIdExternoAsync(notificationFaturaWebHook.Data.Id);
        if (fatura == null)
        {
            return;
        }

        //fatura.PagarWebHook();
        await _faturaContasAReceberRepository.UpdateAsync(fatura);

        if (fatura.Fatura != null && fatura.Fatura.PedidoId.HasValue)
        {
            await _updateStatusPedidoService.UpdateStatusPedidoAsync(new()
            {
                PedidoId = fatura.Fatura.PedidoId.Value,
                StatusPedido = StatusPedido.Faturado
            });
        }

        await _contasAReceberService.VerificarFechamentoAsync(fatura.FaturaId);
    }

    public async Task<ParcelaViewModel> EditarAsync(ParcelaEditDto parcelaEditDto)
    {
        var parcela = await _faturaContasAReceberRepository.GetByIdAsync(parcelaEditDto.Id)
            ?? throw new ExceptionApi("Não foi possível localizar a parcela");

        parcela.Edit(
            dataDeVencimento: parcelaEditDto.DataDeVencimento,
            meioDePagamento: parcelaEditDto.MeioDePagamento,
            valor: parcelaEditDto.Valor,
            desconto: parcelaEditDto.Desconto,
            observacao: parcela.Observacao);

        await _faturaContasAReceberRepository.UpdateAsync(parcela);

        return (ParcelaViewModel)parcela;
    }

    public async Task<ParcelaViewModel> EditAsync(FaturaEdit faturaAReceberEdit)
    {
        var fatura = await _faturaContasAReceberRepository.GetByIdAsync(faturaAReceberEdit.Id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");

        fatura.Edit(
            dataDeVencimento: faturaAReceberEdit.DataDeVencimento,
            meioDePagamento: faturaAReceberEdit.MeioDePagamento,
            valor: faturaAReceberEdit.Valor,
            desconto: faturaAReceberEdit.Desconto,
            observacao: faturaAReceberEdit.Observacao);

        await _faturaContasAReceberRepository.UpdateAsync(fatura);

        return (ParcelaViewModel)fatura;
    }

    public async Task<bool> EstornarAsync(Guid id)
    {
        var parcela = await _faturaContasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a parcela!");

        var transacao = parcela.Estornar();

        parcela.Fatura = null!;
        parcela.Transacoes = null;
        await _faturaContasAReceberRepository.AdicionarTransacaoAsync(transacao);
        await _faturaContasAReceberRepository.UpdateAsync(parcela);

        await _contasAReceberService.VerificarFechamentoAsync(parcela.FaturaId);

        return true;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var parcela = await _faturaContasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a parcela");

        if (!string.IsNullOrWhiteSpace(parcela.IdExterno))
        {
            throw new ExceptionApi("Não é possível excluir uma parcela com integração com o mercado pago!");
        }

        await _faturaContasAReceberRepository.DeleteAsync(parcela);

        return true;
    }

    public async Task<IList<ParcelaPagaDashBoardModel>> FaturasDashBoardAsync()
    {
        var transacoes = await _transacaoFinanceiraRepository.SumTotalMesesAsync(TipoFaturaEnum.A_Receber);
        var faturasPagaDashBoardModel = new List<ParcelaPagaDashBoardModel>();
        foreach (var item in transacoes)
        {
            faturasPagaDashBoardModel.Add(new ParcelaPagaDashBoardModel()
            {
                Mes = item.Key.ConverterMesIntEmNome(),
                Count = item.Value
                    .Where(x => x.Parcela != null &&
                        x.Parcela.Fatura.Tipo == TipoFaturaEnum.A_Receber &&
                        !x.EhEstorno)
                    .Sum(x => x.Valor)
            });
        }
        return faturasPagaDashBoardModel;
    }

    public async Task<ParcelaViewModel> GetByIdAsync(Guid id)
    {
        var parcela = await _faturaContasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a parcela!");

        return (ParcelaViewModel)parcela;
    }

    public async Task<IList<ParcelaViewModel>> GetByPedidoIdAsync(Guid pedidoId)
    {
        var faturas = await _faturaContasAReceberRepository.GetByPedidoIdAsync(pedidoId);
        return faturas.Select(x => (ParcelaViewModel)x).ToList();
    }

    public async Task<decimal> GetSumAReceberAsync()
    {
        var parcelas = await _faturaContasAReceberRepository.ListaParcelasTotalizadorAsync(TipoFaturaEnum.A_Receber);
        return parcelas.Sum(x => x.ValorAPagarAReceber);
    }

    public async Task<ParcelaViewModel> PagarAsync(PagarParcelaDto pagarFaturaAReceberDto)
    {
        pagarFaturaAReceberDto.Validar();

        var parcela = await _faturaContasAReceberRepository.GetByIdAsync(pagarFaturaAReceberDto.Id)
            ?? throw new ExceptionApi("Não foi possível localizar a parcela!");

        var transacao = parcela.Pagar(
            valor: pagarFaturaAReceberDto.Valor,
            meioDePagamento: pagarFaturaAReceberDto.MeioDePagamento,
            observacao: pagarFaturaAReceberDto.Observacao,
            dataDePagamento: pagarFaturaAReceberDto.DataDePagamento,
            desconto: pagarFaturaAReceberDto.Desconto);

        parcela.Fatura = null!;
        parcela.Transacoes = null;
        await _faturaContasAReceberRepository.AdicionarTransacaoAsync(transacao);
        await _faturaContasAReceberRepository.UpdateAsync(parcela);

        await _contasAReceberService.VerificarFechamentoAsync(parcela.FaturaId);

        return (ParcelaViewModel)parcela;
    }

    public async Task<PaginacaoViewModel<ParcelaViewModel>> PaginacaoAsync(PaginacaoParcelaDto paginacaoFaturaAReceberDto)
    {
        var paginacao = await _faturaContasAReceberRepository
            .PaginacaoAsync(paginacaoFaturaAReceberDto);

        var pedidosIds = paginacao
            .Values
            .Where(x => x.Fatura.PedidoId.HasValue)
            .Select(x => x.Fatura.PedidoId!.Value)
            .ToList();

        var pedidos = await _pedidoService.GetPedidosAsync(pedidosIds);
        var parcelasViewModel = new List<ParcelaViewModel>();

        foreach (var parcela in paginacao.Values)
        {
            var parcelaViewModel = (ParcelaViewModel)parcela;
            if (parcela.Fatura.PedidoId.HasValue && pedidos.TryGetValue(parcela.Fatura.PedidoId.Value, out var pedido))
            {
                parcelaViewModel.NumeroDoPedido = pedido.Numero;
            }
            parcelasViewModel.Add(parcelaViewModel);
        }

        return new PaginacaoViewModel<ParcelaViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            Values = parcelasViewModel,
            TotalPaginas = paginacao.TotalPaginas,
        };
    }
}
