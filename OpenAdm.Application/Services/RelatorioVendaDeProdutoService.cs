using OpenAdm.Application.Dtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class RelatorioVendaDeProdutoService : IRelatorioVendaDeProdutoService
{
    private readonly IRelatorioVendaDeProdutoRepository _relatorioVendaDeProdutoRepository;

    public RelatorioVendaDeProdutoService(IRelatorioVendaDeProdutoRepository relatorioVendaDeProdutoRepository)
    {
        _relatorioVendaDeProdutoRepository = relatorioVendaDeProdutoRepository;
    }

    public async Task<RelatorioVendaDeProdutoViewModel> ListarAsync(
        RelatorioVendaDeProdutoDTO relatorioVendaDeProdutoDto)
    {
        var (Dados, TotalPagina) = await _relatorioVendaDeProdutoRepository
            .ListarAsync(dataInicial: relatorioVendaDeProdutoDto.ObterDataInicial(),
                dataFinal: relatorioVendaDeProdutoDto.DataFinal,
                skip: relatorioVendaDeProdutoDto.Skip,
                take: 50,asc: relatorioVendaDeProdutoDto.Asc);

        return new RelatorioVendaDeProdutoViewModel()
        {
            Dados = Dados,
            TotalPagina = TotalPagina,
        };
    }
}