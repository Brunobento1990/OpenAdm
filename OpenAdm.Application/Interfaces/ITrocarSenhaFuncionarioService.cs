using OpenAdm.Application.Dtos.Funcionarios;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ITrocarSenhaFuncionarioService
{
    Task<ResultPartner<ResultadoPadraoViewModel>> TrocarAsync(TrocarSenhaFuncionarioDto dto);
}
