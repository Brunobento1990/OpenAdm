using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.Repositories;

public class LoginFuncionarioRepository(ParceiroContext parceiroContext)
    : ILoginFuncionarioRepository
{
    private readonly ParceiroContext _parceiroContext = parceiroContext;

    public async Task<Funcionario?> GetFuncionarioByEmailAsync(string email)
    {
        return await _parceiroContext
            .Funcionarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}
