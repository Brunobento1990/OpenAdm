using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Factories.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class LoginFuncionarioRepository(IParceiroContextFactory parceiroContextFactory)
    : ILoginFuncionarioRepository
{
    private readonly IParceiroContextFactory _parceiroContextFactory = parceiroContextFactory;

    public async Task<Funcionario?> GetFuncionarioByEmailAsync(string email)
    {
        var context = await _parceiroContextFactory
            .CreateParceiroContextAsync();

        return await context
            .Funcionarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}
