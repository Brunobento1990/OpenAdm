using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class FuncionarioRepository(AppDbContext appDbContext)
    : GenericBaseRepository<Funcionario>(appDbContext), IFuncionarioRepository
{
    public Task<Funcionario?> ObterPorIdAsync(Guid id, Guid parceiroId)
    {
        return AppDbContext.Funcionarios
            .FirstOrDefaultAsync(x => x.Id == id && x.ParceiroId == parceiroId && x.Ativo);
    }
}
