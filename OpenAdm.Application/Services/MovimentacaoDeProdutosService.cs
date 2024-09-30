using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Application.Dtos.MovimentosDeProdutos;
using System.Text;

namespace OpenAdm.Application.Services;

public class MovimentacaoDeProdutosService : IMovimentacaoDeProdutosService
{
    private readonly IMovimentacaoDeProdutoRepository _movimentacaoDeProdutorepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPesoRepository _pesoRepository;
    private readonly ITamanhoRepository _tamanhoRepository;
    private readonly IMovimentacaoDeProdutoRelatorioService _movimentacaoDeProdutoRelatorioService;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    public MovimentacaoDeProdutosService(
        IMovimentacaoDeProdutoRepository movimentacaoDeProdutorepository,
        IProdutoRepository produtoRepository,
        IPesoRepository pesoRepository,
        ITamanhoRepository tamanhoRepository,
        IMovimentacaoDeProdutoRelatorioService movimentacaoDeProdutoRelatorioService,
        IParceiroAutenticado parceiroAutenticado,
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository)
    {
        _movimentacaoDeProdutorepository = movimentacaoDeProdutorepository;
        _produtoRepository = produtoRepository;
        _pesoRepository = pesoRepository;
        _tamanhoRepository = tamanhoRepository;
        _movimentacaoDeProdutoRelatorioService = movimentacaoDeProdutoRelatorioService;
        _parceiroAutenticado = parceiroAutenticado;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
    }

    public async Task<byte[]> GerarRelatorioAsync(RelatorioMovimentoDeProdutoDto relatorioMovimentoDeProdutoDto)
    {
        var configuracaoDePedido = await _configuracoesDePedidoRepository
            .GetConfiguracoesDePedidoAsync();
        var logo = configuracaoDePedido?.Logo is null ? null : Encoding.UTF8.GetString(configuracaoDePedido.Logo);
        var movimentacoes = await _movimentacaoDeProdutorepository.RelatorioAsync(
            dataInicial: relatorioMovimentoDeProdutoDto.DataInicial,
            dataFinal: relatorioMovimentoDeProdutoDto.DataFinal,
            produtosIds: relatorioMovimentoDeProdutoDto.ProdutosId,
            pesosIds: relatorioMovimentoDeProdutoDto.PesosId,
            tamanhosIds: relatorioMovimentoDeProdutoDto.TamanhosId);

        var produtosids = movimentacoes
            .DistinctBy(x => x.ProdutoId)
            .Select(x => x.ProdutoId)
            .ToList();

        var tamanhosIds = movimentacoes
            .Where(x => x.TamanhoId != null)
            .DistinctBy(x => x.TamanhoId)
            .Select(x => x.TamanhoId!.Value)
            .ToList();

        var pesosIds = movimentacoes
            .Where(x => x.PesoId != null)
            .DistinctBy(x => x.PesoId)
            .Select(x => x.PesoId!.Value)
            .ToList();

        var produtos = await _produtoRepository.GetDictionaryProdutosAsync(produtosids);
        var tamanhos = await _tamanhoRepository.GetDescricaoTamanhosAsync(tamanhosIds);
        var pesos = await _pesoRepository.GetDescricaoPesosAsync(pesosIds);

        var movimentacoesRelatorio = new List<MovimentacaoDeProdutoRelatorio>();
        IList<RelatorioMovimentoDeProdutoTotalizacaoDto> totalCategorias = [];
        IList<RelatorioMovimentoDeProdutoTotalizacaoDto> totalPesos = [];
        IList<RelatorioMovimentoDeProdutoTotalizacaoDto> totalTamanhos = [];

        foreach (var item in movimentacoes)
        {
            if (produtos.TryGetValue(item.ProdutoId, out var produto))
            {
                var totalCategoria = totalCategorias.FirstOrDefault(x => x.Descricao == produto.Categoria.Descricao);

                if(totalCategoria == null)
                {
                    totalCategorias.Add(new RelatorioMovimentoDeProdutoTotalizacaoDto()
                    {
                        Descricao = produto.Categoria.Descricao,
                        Total = 1
                    });
                }
                else
                {
                    totalCategoria.Total++;
                }

                var pesoTamanho = string.Empty;

                if (item.TamanhoId.HasValue && tamanhos.TryGetValue(item.TamanhoId.Value, out var tamanho))
                {
                    pesoTamanho = tamanho;
                }

                if (item.PesoId.HasValue && pesos.TryGetValue(item.PesoId.Value, out var peso))
                {
                    pesoTamanho = peso;
                }

                movimentacoesRelatorio.Add(new()
                {
                    Categoria = produto.Categoria.Descricao,
                    DataDaMovimentacao = item.DataDeCriacao,
                    Descricao = produto.Descricao,
                    Quantidade = item.QuantidadeMovimentada,
                    Referencia = produto.Referencia,
                    TipoMovimento = item.TipoMovimentacaoDeProduto.ToString(),
                    PesoTamanho = pesoTamanho
                });
            }
        }

        var groupedByPesos = movimentacoes
             .Where(x => x.PesoId != null)
             .GroupBy(p => p.PesoId)
             .Select(group => new
             {
                 Peso = group.Key!.Value,
                 Count = group.Count()
             });

        var groupedByTamanhos = movimentacoes
             .Where(x => x.TamanhoId != null)
             .GroupBy(p => p.TamanhoId)
             .Select(group => new
             {
                 Tamanho = group.Key!.Value,
                 Count = group.Count()
             });

        foreach (var peso in groupedByPesos)
        {
            if (pesos.TryGetValue(peso.Peso, out var pes))
            {
                totalPesos.Add(new RelatorioMovimentoDeProdutoTotalizacaoDto()
                {
                    Descricao = pes,
                    Total = peso.Count
                });
            }
        }

        foreach (var tamanho in groupedByTamanhos)
        {
            if (tamanhos.TryGetValue(tamanho.Tamanho, out var tama))
            {
                totalTamanhos.Add(new RelatorioMovimentoDeProdutoTotalizacaoDto()
                {
                    Descricao = tama,
                    Total = tamanho.Count
                });
            }
        }

        return _movimentacaoDeProdutoRelatorioService.ObterPdfAsync(
            movimentacoesRelatorio,
            _parceiroAutenticado.NomeFantasia,
            relatorioMovimentoDeProdutoDto.DataInicial,
            relatorioMovimentoDeProdutoDto.DataFinal,
            logo,
            totalCategoria: totalCategorias,
            totalPesos: totalPesos,
            totalTamanhos: totalTamanhos);
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

    public async Task MovimentarItensPedidoAsync(IList<ItemPedido> itens)
    {
        var data = DateTime.Now;
        var movimentos = itens.Select(x => new MovimentacaoDeProduto(
            id: Guid.NewGuid(),
            dataDeCriacao: data,
            dataDeAtualizacao: data,
            numero: 0,
            quantidadeMovimentada: x.Quantidade,
            tipoMovimentacaoDeProduto: TipoMovimentacaoDeProduto.Saida,
            produtoId: x.ProdutoId,
            tamanhoId: x.TamanhoId,
            pesoId: x.PesoId,
            observacao: null
            )).ToList();
        await _movimentacaoDeProdutorepository.AddRangeAsync(movimentos);
    }

    public async Task<IList<MovimentoDeProdutoDashBoardModel>> MovimentoDashBoardAsync()
    {
        var movimentos = await _movimentacaoDeProdutorepository.CountTresMesesAsync();

        var movimentosDash = new List<MovimentoDeProdutoDashBoardModel>();

        foreach (var item in movimentos)
        {
            movimentosDash.Add(new MovimentoDeProdutoDashBoardModel()
            {
                Mes = item.Key.ConverterMesIntEmNome(),
                Count = item.Value
            });
        }

        return movimentosDash;
    }
}
