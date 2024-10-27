using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.FaturasModel;
using OpenAdm.Application.Models.ParcelasModel;
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
    private readonly IUpdateStatusPedidoService _pedidoService;
    public ParcelaService(
        IParcelaRepository faturaContasAReceberRepository,
        IFaturaService contasAReceberService,
        IUpdateStatusPedidoService pedidoService)
    {
        _faturaContasAReceberRepository = faturaContasAReceberRepository;
        _contasAReceberService = contasAReceberService;
        _pedidoService = pedidoService;
    }

    public async Task BaixarFaturaWebHookAsync(NotificationFaturaWebHook notificationFaturaWebHook)
    {
        var fatura = await _faturaContasAReceberRepository.GetByIdExternoAsync(notificationFaturaWebHook.Data.Id);
        if (fatura == null || fatura.Status == StatusParcelaEnum.Pago)
        {
            return;
        }

        fatura.PagarWebHook();
        await _faturaContasAReceberRepository.UpdateAsync(fatura);

        if (fatura.Fatura != null && fatura.Fatura.PedidoId.HasValue)
        {
            await _pedidoService.UpdateStatusPedidoAsync(new()
            {
                PedidoId = fatura.Fatura.PedidoId.Value,
                StatusPedido = StatusPedido.Faturado
            });
        }

        await _contasAReceberService.VerificarFechamentoAsync(fatura.FaturaId);
    }

    public async Task<ParcelaViewModel> EditAsync(FaturaEdit faturaAReceberEdit)
    {
        var fatura = await _faturaContasAReceberRepository.GetByIdAsync(faturaAReceberEdit.Id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");

        fatura.Edit(
            status: faturaAReceberEdit.Status,
            dataDeVencimento: faturaAReceberEdit.DataDeVencimento,
            dataDePagamento: faturaAReceberEdit.DataDePagamento,
            meioDePagamento: faturaAReceberEdit.MeioDePagamento,
            valor: faturaAReceberEdit.Valor,
            desconto: faturaAReceberEdit.Desconto,
            observacao: faturaAReceberEdit.Observacao);

        await _faturaContasAReceberRepository.UpdateAsync(fatura);

        return (ParcelaViewModel)fatura;
    }

    public async Task<IList<ParcelaPagaDashBoardModel>> FaturasDashBoardAsync()
    {
        var faturas = await _faturaContasAReceberRepository.SumTotalMesesAsync(TipoFaturaEnum.A_Receber);
        var faturasPagaDashBoardModel = new List<ParcelaPagaDashBoardModel>();
        foreach (var item in faturas)
        {
            faturasPagaDashBoardModel.Add(new ParcelaPagaDashBoardModel()
            {
                Mes = item.Key.ConverterMesIntEmNome(),
                Count = item.Value
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

    public async Task<IList<ParcelaViewModel>> GetByPedidoIdAsync(Guid pedidoId, StatusParcelaEnum? statusFaturaContasAReceberEnum)
    {
        var faturas = await _faturaContasAReceberRepository.GetByPedidoIdAsync(pedidoId, statusFaturaContasAReceberEnum);
        return faturas.Select(x => (ParcelaViewModel)x).ToList();
    }

    public Task<decimal> GetSumAReceberAsync()
        => _faturaContasAReceberRepository.SumTotalAsync(TipoFaturaEnum.A_Receber);

    public async Task<ParcelaViewModel> PagarAsync(PagarParcelaDto pagarFaturaAReceberDto)
    {
        var parcela = await _faturaContasAReceberRepository.GetByIdAsync(pagarFaturaAReceberDto.Id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");

        parcela.Pagar(
            desconto: pagarFaturaAReceberDto.Desconto,
            meioDePagamento: pagarFaturaAReceberDto.MeioDePagamento,
            observacao: pagarFaturaAReceberDto.Observacao);

        await _faturaContasAReceberRepository.UpdateAsync(parcela);
        parcela.Fatura = null!;

        await _contasAReceberService.VerificarFechamentoAsync(parcela.FaturaId);

        return (ParcelaViewModel)parcela;
    }

    public async Task<PaginacaoViewModel<ParcelaViewModel>> PaginacaoAsync(PaginacaoParcelaDto paginacaoFaturaAReceberDto)
    {
        var paginacao = await _faturaContasAReceberRepository
            .PaginacaoAsync(paginacaoFaturaAReceberDto);

        return new PaginacaoViewModel<ParcelaViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            Values = paginacao.Values.Select(x => (ParcelaViewModel)x).ToList(),
            TotalPaginas = paginacao.TotalPaginas,
        };
    }
}
