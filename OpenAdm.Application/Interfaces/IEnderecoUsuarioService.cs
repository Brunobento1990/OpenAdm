using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Application.Interfaces;

public interface IEnderecoUsuarioService
{
    Task<EnderecoViewModel> CriarOuEditarAsync(EnderecoDto enderecoDto);
}
