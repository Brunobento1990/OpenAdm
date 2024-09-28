using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class ProdutosMaisVendidosService : IProdutosMaisVendidosService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutosMaisVendidosRepository _produtosMaisVendidosRepository;

    public ProdutosMaisVendidosService(IProdutoRepository produtoRepository, IProdutosMaisVendidosRepository produtosMaisVendidosRepository)
    {
        _produtoRepository = produtoRepository;
        _produtosMaisVendidosRepository = produtosMaisVendidosRepository;
    }

    public async Task<IList<ProdutoViewModel>> GetProdutosMaisVendidosAsync()
    {
        var produtosMaisVendidos = await _produtoRepository.GetProdutosMaisVendidosAsync();
        return produtosMaisVendidos.Select(x => new ProdutoViewModel().ToModel(x)).ToList();
    }

    public async Task ProcessarAsync(Pedido pedido)
    {
        var produtosIds = pedido.ItensPedido
            .Select(x => x.ProdutoId)
            .ToList();

        var produtosMaisVendidos = await _produtosMaisVendidosRepository.GetProdutosMaisVendidosAsync(produtosIds);

        var add = new List<ProdutoMaisVendido>();
        var updates = new List<ProdutoMaisVendido>();
        var date = DateTime.Now;

        foreach (var itens in pedido.ItensPedido)
        {
            var addOrUpdate = produtosMaisVendidos
                .FirstOrDefault(x => x.ProdutoId == itens.ProdutoId);

            if (addOrUpdate == null)
            {
                var insert = add.FirstOrDefault(x => x.ProdutoId == itens.ProdutoId);

                if (insert == null)
                {
                    addOrUpdate = new ProdutoMaisVendido(
                        Guid.NewGuid(),
                        date,
                        date,
                        0,
                        itens.Quantidade,
                        1,
                        itens.ProdutoId);

                    add.Add(addOrUpdate);
                }
                else
                {
                    insert.UpdateQuantidadeProdutos(itens.Quantidade);
                }

            }
            else
            {
                var update = updates.FirstOrDefault(x => x.ProdutoId == itens.ProdutoId);

                if (update == null)
                {
                    addOrUpdate.UpdateQuantidadeProdutos(itens.Quantidade);
                    updates.Add(addOrUpdate);
                }
                else
                {
                    update.UpdateQuantidadeProdutos(itens.Quantidade);
                }

            }
        }

        await _produtosMaisVendidosRepository.AddRangeAsync(add);
        await _produtosMaisVendidosRepository.UpdateRangeAsync(updates);
    }
}
