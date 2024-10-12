using OpenAdm.Application.Dtos.ContasAReceberDto;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ContasAReceberModel;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public sealed class FaturaContasAReceberService : IFaturaContasAReceberService
{
    private readonly IFaturaContasAReceberRepository _faturaContasAReceberRepository;

    public FaturaContasAReceberService(IFaturaContasAReceberRepository faturaContasAReceberRepository)
    {
        _faturaContasAReceberRepository = faturaContasAReceberRepository;
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
