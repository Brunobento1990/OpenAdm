using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;
using System.Reactive.Linq;
using System.Text;

namespace OpenAdm.Application.Services;

public class PedidoService(
    IPedidoRepository pedidoRepository,
    IItensPedidoRepository itensPedidoRepository,
    IPdfPedidoService pdfPedidoService,
    IParceiroAutenticado parceiroAutenticado,
    IConfiguracoesDePedidoRepository configuracoesDePedidoRepository)
    : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;
    private readonly IItensPedidoRepository _itensPedidoRepository = itensPedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService = pdfPedidoService;
    private readonly IParceiroAutenticado _parceiroAutenticado = parceiroAutenticado;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository = configuracoesDePedidoRepository;

    public async Task<PedidoViewModel> GetAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new Exception($"Pedido não localizado: {pedidoId}");
        var pedidoViewModel = new PedidoViewModel().ForModel(pedido);

        return pedidoViewModel;
    }

    public async Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(PaginacaoPedidoDto paginacaoPedidoDto)
    {
        var paginacao = await _pedidoRepository.PaginacaoAsync(paginacaoPedidoDto);

        return new PaginacaoViewModel<PedidoViewModel>()
        {
            TotalPaginas = paginacao.TotalPaginas,
            TotalDeRegistros = paginacao.TotalDeRegistros,
            Values = paginacao.Values.Select(x => new PedidoViewModel().ForModel(x)).ToList()
        };
    }

    public async Task<IDictionary<Guid, PedidoViewModel>> GetPedidosAsync(IList<Guid> ids)
    {
        var pedidos = await _pedidoRepository.GetPedidosAsync(ids);
        var pedidosViewModel = new Dictionary<Guid, PedidoViewModel>();

        foreach (var pedido in pedidos)
        {
            pedidosViewModel.TryAdd(pedido.Key, new PedidoViewModel().ForModelPedidoEmAberto(pedido.Value));
        }

        return pedidosViewModel;
    }

    public async Task<IList<PedidoViewModel>> GetPedidosEmAbertAsync()
    {
        var pedidos = await _pedidoRepository.GetPedidosEmAbertoAsync();
        return pedidos.Select(x => new PedidoViewModel().ForModelPedidoEmAberto(x)).ToList();
    }

    public async Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido, Guid usuarioId)
    {
        var pedidos = await _pedidoRepository.GetPedidosByUsuarioIdAsync(usuarioId, statusPedido);
        return pedidos
            .Select(x => new PedidoViewModel().ForModel(x))
            .ToList();
    }

    public async Task<byte[]> PedidoProducaoAsync(RelatorioProducaoDto relatorioProducaoDto)
    {
        var configuracaoDePedido = await _configuracoesDePedidoRepository
            .GetConfiguracoesDePedidoAsync();
        var logo = configuracaoDePedido?.Logo is null ? null : Encoding.UTF8.GetString(configuracaoDePedido.Logo);

        var itens = await _itensPedidoRepository.GetItensPedidoByProducaoAsync(
            pedidosIds: relatorioProducaoDto.PedidosIds,
            produtosIds: relatorioProducaoDto.ProdutosIds,
            pesosIds: relatorioProducaoDto.PesosIds,
            tamanhosIds: relatorioProducaoDto.TamanhosIds);

        if (itens.Count == 0)
        {
            throw new ExceptionApi("Não há produtos a serem produzidos!");
        }

        var itensProducao = new List<ItemPedidoProducaoViewModel>();
        var pedidos = itens.DistinctBy(x => x.PedidoId).Select(x => $"#{x.Pedido.Numero} - {x.Pedido.Usuario.Nome}").ToList();

        foreach (var item in itens)
        {
            var itemProducao = itensProducao.FirstOrDefault(
                x => x.ProdutoId == item.ProdutoId &&
                x.PesoId == item.PesoId &&
                x.TamanhoId == item.TamanhoId);

            if (itemProducao != null)
            {
                itemProducao.Quantidade += item.Quantidade;
                continue;
            }

            itensProducao.Add(new ItemPedidoProducaoViewModel()
            {
                Id = item.Id,
                Quantidade = item.Quantidade,
                Categoria = item.Produto.Categoria.Descricao,
                Peso = item.Peso?.Descricao ?? string.Empty,
                PesoId = item.PesoId,
                Produto = item.Produto.Descricao,
                ProdutoId = item.ProdutoId,
                Referencia = item.Produto.Referencia,
                Tamanho = item.Tamanho?.Descricao ?? string.Empty,
                TamanhoId = item.TamanhoId
            });
        }

        return _pdfPedidoService.ProducaoPedido(
            itensProducao.OrderBy(x => x.Produto).ToList(),
            _parceiroAutenticado.NomeFantasia,
            logo,
            pedidos);
    }
}
