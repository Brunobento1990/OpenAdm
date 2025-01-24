using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class DeleteProdutoCarrinhoService : IDeleteProdutoCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    public DeleteProdutoCarrinhoService(ICarrinhoRepository carrinhoRepository, IUsuarioAutenticado usuarioAutenticado)
    {
        _carrinhoRepository = carrinhoRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<bool> DeleteProdutoCarrinhoAsync(Guid produtoId)
    {
        var key = _usuarioAutenticado.Id.ToString();
        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(key);

        if (carrinho.UsuarioId == Guid.Empty)
            carrinho.UsuarioId = _usuarioAutenticado.Id;

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
