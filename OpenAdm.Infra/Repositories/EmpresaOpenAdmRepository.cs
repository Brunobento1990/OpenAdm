using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Helpers;

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
            .LinksEmpresas
            .AsNoTracking()
            .Where(x => x.Url == origem)
            .Select(x => x.Empresa)
            .FirstOrDefaultAsync(x => x.Ativo);
    }

    public async Task<EmpresaOpenAdm?> ObterPorIdAsync(Guid id)
    {
        return await _appDbContext
            .Empresas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.Ativo);
    }

}
