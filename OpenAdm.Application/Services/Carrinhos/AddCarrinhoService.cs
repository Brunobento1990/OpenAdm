using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class AddCarrinhoService : IAddCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;

    public AddCarrinhoService(ICarrinhoRepository carrinhoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
    }

    public async Task<bool> AddCarrinhoAsync(IList<AddCarrinhoModel> addCarrinhoModel, UsuarioViewModel usuarioViewModel)
    {
        var _key = usuarioViewModel.Id.ToString();
        var carrinho = await _carrinhoRepository.GetCarrinhoAsync(_key);

        if (carrinho.UsuarioId == Guid.Empty)
            carrinho.UsuarioId = Guid.Parse(_key);

        foreach (var item in addCarrinhoModel)
        {
            var addProduto = carrinho?
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

                carrinho?.Produtos.Add(addProduto);
            }
            else
            {
                addProduto.Quantidade += item.Quantidade;
            }
        }

        await _carrinhoRepository.AdicionarProdutoAsync(carrinho, _key);

        return true;
    }
}
