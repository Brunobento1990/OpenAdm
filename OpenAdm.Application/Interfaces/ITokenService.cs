using OpenAdm.Application.Models;

namespace OpenAdm.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(object obj, ConfiguracaoDeToken configGenerateToken);
    bool IsFuncionario();
}
