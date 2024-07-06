using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class MovimentacaoDeProdutosService : IMovimentacaoDeProdutosService
{
    private readonly IMovimentacaoDeProdutoRepository _movimentacaoDeProdutorepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPesoRepository _pesoRepository;
    private readonly ITamanhoRepository _tamanhoRepository;

    public MovimentacaoDeProdutosService(
        IMovimentacaoDeProdutoRepository movimentacaoDeProdutorepository,
        IProdutoRepository produtoRepository,
        IPesoRepository pesoRepository,
        ITamanhoRepository tamanhoRepository)
    {
        _movimentacaoDeProdutorepository = movimentacaoDeProdutorepository;
        _produtoRepository = produtoRepository;
        _pesoRepository = pesoRepository;
        _tamanhoRepository = tamanhoRepository;
    }

    public async Task<PaginacaoViewModel<MovimentacaoDeProdutoViewModel>> 
        GetPaginacaoAsync(PaginacaoMovimentacaoDeProdutoDto paginacaoMovimentacaoDeProdutoDto)
    {
        var paginacao = await _movimentacaoDeProdutorepository
            .GetPaginacaoMovimentacaoDeProdutoAsync(paginacaoMovimentacaoDeProdutoDto);

        var produtosids = paginacao
            .Values
            .DistinctBy(x => x.ProdutoId)
            .Select(x => x.ProdutoId)
            .ToList();

        var tamanhosIds = paginacao
            .Values.Where(x => x.TamanhoId != null)
            .DistinctBy(x => x.TamanhoId)
            .Select(x => x.TamanhoId!.Value)
            .ToList();

        var pesosIds = paginacao
            .Values
            .Where(x => x.PesoId != null)
            .DistinctBy(x => x.PesoId)
            .Select(x => x.PesoId!.Value)
            .ToList();

        var produtos = await _produtoRepository.GetDescricaoDeProdutosAsync(produtosids);
        var tamanhos = await _tamanhoRepository.GetDescricaoTamanhosAsync(tamanhosIds);
        var pesos = await _pesoRepository.GetDescricaoPesosAsync(pesosIds);

        return new PaginacaoViewModel<MovimentacaoDeProdutoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao
                .Values
                .Select(x =>
                    new MovimentacaoDeProdutoViewModel()
                        .ToModel(x,
                            produtos.FirstOrDefault(p => p.Key == x.ProdutoId).Value,
                            pesos.FirstOrDefault(p => p.Key == x.PesoId).Value,
                            tamanhos.FirstOrDefault(t => t.Key == x.TamanhoId).Value))
                .ToList()
        };
    }
}
