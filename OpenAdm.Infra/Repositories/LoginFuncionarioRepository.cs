using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Factories.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class LoginFuncionarioRepository(IContextFactory contextFactory)
    : ILoginFuncionarioRepository
{
    private readonly IContextFactory _contextFactory = contextFactory;
    public async Task<Funcionario?> GetFuncionarioByEmailAsync(string email)
    {
        var context = await _contextFactory
            .GetParceiroContextAsync();

        return await context
            .Funcionarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}
