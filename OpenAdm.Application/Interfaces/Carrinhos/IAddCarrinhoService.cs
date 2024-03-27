using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Application.Interfaces.Carrinhos;

public interface IAddCarrinhoService
{
    Task<bool> AddCarrinhoAsync(IList<AddCarrinhoModel> addCarrinhoModel);
}
