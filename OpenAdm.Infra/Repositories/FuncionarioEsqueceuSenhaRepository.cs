using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OpenAdm.Infra.Repositories;

public class FuncionarioEsqueceuSenhaRepository(AppDbContext appDbContext)
    : GenericBaseRepository<FuncionarioEsqueceuSenha>(appDbContext), IFuncionarioEsqueceuSenhaRepository
{
    public Task<FuncionarioEsqueceuSenha?> ObterPorTokenAsync(Guid token, Guid parceiroId)
    {
        return AppDbContext.FuncionariosEsqueceramSenha
            .Include(x => x.Funcionario)
            .FirstOrDefaultAsync(x => x.Token == token && x.ParceiroId == parceiroId);
    }
}
