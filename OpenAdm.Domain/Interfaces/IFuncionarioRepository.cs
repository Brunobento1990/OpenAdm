using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IFuncionarioRepository : IGenericBaseRepository<Funcionario>
{
    Task<Funcionario?> ObterPorIdAsync(Guid id, Guid parceiroId);
}
