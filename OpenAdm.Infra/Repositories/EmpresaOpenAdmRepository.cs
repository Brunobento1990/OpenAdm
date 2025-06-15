using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class EmpresaOpenAdmRepository : IEmpresaOpenAdmRepository
{
    private readonly AppDbContext _appDbContext;

    public EmpresaOpenAdmRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<EmpresaOpenAdm?> ObterPorOrigemAsync(string origem)
    {
        return await _appDbContext
            .Empresas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => (x.UrlEcommerce == origem || x.UrlAdmin == origem) && x.Ativo);
    }
}
