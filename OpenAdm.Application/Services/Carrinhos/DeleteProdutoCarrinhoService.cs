using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class DeleteProdutoCarrinhoService : IDeleteProdutoCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;

    public DeleteProdutoCarrinhoService(ICarrinhoRepository carrinhoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
    }

    public async Task<bool> DeleteProdutoCarrinhoAsync(Guid produtoId, Guid usuarioId)
    {
        var key = usuarioId.ToString();
        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(key);

        if (carrinho.UsuarioId == Guid.Empty)
            carrinho.UsuarioId = usuarioId;

        var produtos = carrinho.Produtos.Where(x => x.ProdutoId == produtoId).ToList();

        foreach (var produto in produtos)
        {
            if (produto != null)
            {
                carrinho.Produtos.Remove(produto);
            }
        }

        await _carrinhoRepository.AdicionarProdutoAsync(carrinho, key);

        return true;
    }
}
