using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface ILoginFuncionarioRepository
{
    Task<Funcionario?> GetFuncionarioByEmailAsync(string email);
}
