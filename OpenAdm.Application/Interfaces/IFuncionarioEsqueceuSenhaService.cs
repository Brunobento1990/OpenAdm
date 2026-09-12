using OpenAdm.Application.Dtos.Funcionarios;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IFuncionarioEsqueceuSenhaService
{
    Task<ResultPartner<ResultadoPadraoViewModel>> SolicitarAsync(EsqueceuSenhaDto esqueceuSenhaDto);
    Task<ResultPartner<ResultadoPadraoViewModel>> RecuperarSenhaAsync(RecuperarSenhaFuncionarioDto dto);
}
