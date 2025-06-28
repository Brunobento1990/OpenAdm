using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.Repositories;

public class LoginFuncionarioRepository
    : ILoginFuncionarioRepository
{
    private readonly AppDbContext _appDbContext;

    public LoginFuncionarioRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Funcionario?> GetFuncionarioByEmailAsync(string email, Guid parceiroId)
    {
        return await _appDbContext
            .Funcionarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email && x.ParceiroId == parceiroId && x.Ativo);
    }
}
