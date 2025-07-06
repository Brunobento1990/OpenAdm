using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Application.Interfaces;

public interface IUsuarioPedidoServico
{
    Task<PaginacaoUltimoPedidoUsuarioViewModel> ListarAsync(PaginacaoUltimoPedidoUsuarioDto paginacaoUltimoPedidoUsuarioDto);
}
