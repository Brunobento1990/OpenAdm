using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ILoginFuncionarioRepository
{
    Task<Funcionario?> GetFuncionarioByEmailAsync(string email, Guid parceiroId);
}
