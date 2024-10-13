using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public sealed class FaturaContasAReceberService : IFaturaContasAReceberService
{
    private readonly IFaturaContasAReceberRepository _faturaContasAReceberRepository;
    private readonly IContasAReceberService _contasAReceberService;
    public FaturaContasAReceberService(IFaturaContasAReceberRepository faturaContasAReceberRepository, IContasAReceberService contasAReceberService)
    {
        _faturaContasAReceberRepository = faturaContasAReceberRepository;
        _contasAReceberService = contasAReceberService;
    }

    public async Task<FaturaContasAReceberViewModel> EditAsync(FaturaAReceberEdit faturaAReceberEdit)
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

        return (FaturaContasAReceberViewModel)fatura;
    }

    public async Task<IList<FaturaPagaDashBoardModel>> FaturasDashBoardAsync()
    {
        var faturas = await _faturaContasAReceberRepository.SumMesesAsync();
        var faturasPagaDashBoardModel = new List<FaturaPagaDashBoardModel>();
        foreach (var item in faturas)
        {
            faturasPagaDashBoardModel.Add(new FaturaPagaDashBoardModel()
            {
                Mes = item.Key.ConverterMesIntEmNome(),
                Count = item.Value
            });
        }
        return faturasPagaDashBoardModel;
    }

    public async Task<FaturaContasAReceberViewModel> GetByIdAsync(Guid id)
    {
        var fatura = await _faturaContasAReceberRepository.GetByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");

        return (FaturaContasAReceberViewModel)fatura;
    }

    public async Task<IList<FaturaContasAReceberViewModel>> GetByPedidoIdAsync(Guid pedidoId, StatusFaturaContasAReceberEnum? statusFaturaContasAReceberEnum)
    {
        var faturas = await _faturaContasAReceberRepository.GetByPedidoIdAsync(pedidoId, statusFaturaContasAReceberEnum);
        return faturas.Select(x => (FaturaContasAReceberViewModel)x).ToList();
    }

    public async Task<FaturaContasAReceberViewModel> PagarAsync(PagarFaturaAReceberDto pagarFaturaAReceberDto)
    {
        var fatura = await _faturaContasAReceberRepository.GetByIdAsync(pagarFaturaAReceberDto.Id)
            ?? throw new ExceptionApi("Não foi possível localizar a fatura!");

        fatura.Pagar(
            desconto: pagarFaturaAReceberDto.Desconto,
            meioDePagamento: pagarFaturaAReceberDto.MeioDePagamento,
            observacao: pagarFaturaAReceberDto.Observacao);

        await _faturaContasAReceberRepository.UpdateAsync(fatura);
        fatura.ContasAReceber = null!;

        await _contasAReceberService.VerificarFechamentoAsync(fatura.ContasAReceberId);

        return (FaturaContasAReceberViewModel)fatura;
    }

    public async Task<PaginacaoViewModel<FaturaContasAReceberViewModel>> PaginacaoAsync(PaginacaoFaturaAReceberDto paginacaoFaturaAReceberDto)
    {
        var paginacao = await _faturaContasAReceberRepository
            .PaginacaoAsync(paginacaoFaturaAReceberDto);

        return new PaginacaoViewModel<FaturaContasAReceberViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => (FaturaContasAReceberViewModel)x).ToList()
        };
    }
}
