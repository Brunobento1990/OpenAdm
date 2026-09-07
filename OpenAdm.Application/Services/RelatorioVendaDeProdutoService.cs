using OpenAdm.Application.Dtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Pdf.Interfaces;

namespace OpenAdm.Application.Services;

public class RelatorioVendaDeProdutoService : IRelatorioVendaDeProdutoService
{
    private readonly IRelatorioVendaDeProdutoRepository _relatorioVendaDeProdutoRepository;
    private readonly IRelatorioVendaDeProdutoPdfService _pdfService;
    private readonly IParceiroAutenticado _parceiroAutenticado;

    public RelatorioVendaDeProdutoService(
        IRelatorioVendaDeProdutoRepository relatorioVendaDeProdutoRepository,
        IRelatorioVendaDeProdutoPdfService pdfService,
        IParceiroAutenticado parceiroAutenticado)
    {
        _relatorioVendaDeProdutoRepository = relatorioVendaDeProdutoRepository;
        _pdfService = pdfService;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<RelatorioVendaDeProdutoViewModel> ListarAsync(
        RelatorioVendaDeProdutoDTO relatorioVendaDeProdutoDto)
    {
        var (dados, totalPagina, quantidadeTotal, valorTotal) = await _relatorioVendaDeProdutoRepository
            .ListarAsync(dataInicial: relatorioVendaDeProdutoDto.ObterDataInicial(),
                dataFinal: relatorioVendaDeProdutoDto.DataFinal,
                skip: relatorioVendaDeProdutoDto.Skip,
                take: 50,asc: relatorioVendaDeProdutoDto.Asc);

        return new RelatorioVendaDeProdutoViewModel()
        {
            Dados = dados,
            TotalPagina = totalPagina,
            Totais = new RelatorioVendaDeProdutoTotaisViewModel
            {
                QuantidadeTotal = quantidadeTotal,
                ValorTotal = valorTotal
            }
        };
    }

    public async Task<byte[]> ImprimirAsync(RelatorioVendaDeProdutoDTO relatorioVendaDeProdutoDto)
    {
        var dataInicial = relatorioVendaDeProdutoDto.ObterDataInicial();
        var (dados, _, quantidadeTotal, valorTotal) = await _relatorioVendaDeProdutoRepository.ListarAsync(
            dataInicial,
            relatorioVendaDeProdutoDto.DataFinal,
            0,
            null,
            relatorioVendaDeProdutoDto.Asc);

        var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();

        return _pdfService.Gerar(
            dados,
            parceiro.NomeFantasia,
            parceiro.Logo,
            dataInicial,
            relatorioVendaDeProdutoDto.DataFinal,
            quantidadeTotal,
            valorTotal);
    }
}
