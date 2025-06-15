using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Domain.Interfaces;

public interface IEmpresaOpenAdmRepository
{
    Task<EmpresaOpenAdm?> ObterPorOrigemAsync(string origem);
}
