using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class ProdutosMaisVendidosService : IProdutosMaisVendidosService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutosMaisVendidosService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<IList<ProdutoViewModel>> GetProdutosMaisVendidosAsync()
    {
        var produtosMaisVendidos = await _produtoRepository.GetProdutosMaisVendidosAsync();
        return produtosMaisVendidos.Select(x => new ProdutoViewModel().ToModel(x)).ToList();
    }
}
