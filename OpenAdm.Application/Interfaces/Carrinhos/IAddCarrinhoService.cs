using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Application.Interfaces.Carrinhos;

public interface IAddCarrinhoService
{
    Task<int> AddCarrinhoAsync(IList<AddCarrinhoModel> addCarrinhoModel);
}
