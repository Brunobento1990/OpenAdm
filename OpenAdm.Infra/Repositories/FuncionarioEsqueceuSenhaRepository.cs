using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class FuncionarioEsqueceuSenhaRepository(AppDbContext appDbContext)
    : GenericBaseRepository<FuncionarioEsqueceuSenha>(appDbContext), IFuncionarioEsqueceuSenhaRepository
{
}
