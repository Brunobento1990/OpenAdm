using OpenAdm.Application.Models.Carrinhos;
using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Application.Interfaces.Carrinhos;

public interface IGetCarrinhoService
{
    Task<IList<CarrinhoViewModel>> GetCarrinhoAsync(UsuarioViewModel usuarioViewModel);
}
