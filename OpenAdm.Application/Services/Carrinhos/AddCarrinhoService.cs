using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class AddCarrinhoService : IAddCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    public AddCarrinhoService(
        ICarrinhoRepository carrinhoRepository, IUsuarioAutenticado usuarioAutenticado)
    {
        _carrinhoRepository = carrinhoRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<int> AddCarrinhoAsync(IList<AddCarrinhoModel> addCarrinhoModel)
    {
        var key = _usuarioAutenticado.Id.ToString();
        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(key);

        if (carrinho.UsuarioId == Guid.Empty)
            carrinho.UsuarioId = _usuarioAutenticado.Id;

        foreach (var item in addCarrinhoModel)
        {
            var addProduto = carrinho
                .Produtos
                .FirstOrDefault(x =>
                    x.ProdutoId == item.ProdutoId &&
                    x.PesoId == item.PesoId &&
                    x.TamanhoId == item.TamanhoId);

            if (addProduto == null)
            {
                addProduto = new()
                {
                    ProdutoId = item.ProdutoId,
                    TamanhoId = item.TamanhoId,
                    PesoId = item.PesoId,
                    Quantidade = item.Quantidade
                };

                carrinho.Produtos.Add(addProduto);
            }
            else
            {
                addProduto.Quantidade += item.Quantidade;
            }
        }

        await _carrinhoRepository.AdicionarProdutoAsync(carrinho, key);

        return carrinho.Produtos.Count;
    }
}
