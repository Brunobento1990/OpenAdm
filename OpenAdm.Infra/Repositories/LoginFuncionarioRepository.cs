using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

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
