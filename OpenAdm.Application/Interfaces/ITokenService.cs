using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Interfaces;

public interface ITokenService
{
    string GenerateTokenFuncionario(Funcionario funcionario);
    bool IsFuncionario();
}
