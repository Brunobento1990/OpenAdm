using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IFuncionarioEsqueceuSenhaRepository : IGenericBaseRepository<FuncionarioEsqueceuSenha>
{
    Task<FuncionarioEsqueceuSenha?> ObterPorTokenAsync(Guid token, Guid parceiroId);
}
