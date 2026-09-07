using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Helpers;

namespace OpenAdm.Worker.Infra.Repositories;

public class EmpresaOpenAdmRepository : IEmpresaOpenAdmRepository
{
    private readonly AppDbContext _context;

    public EmpresaOpenAdmRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<EmpresaOpenAdm?> ObterPorOrigemAsync(string origem)
    {
        var dominio = DominioEmpresaHelper.Normalizar(origem);
        return _context.LinksEmpresas
            .AsNoTracking()
            .Where(x =>
                x.Url == dominio &&
                x.Empresa.Ativo)
            .Select(x => x.Empresa)
            .FirstOrDefaultAsync();
    }

    public async Task<EmpresaOpenAdm?> ObterPorIdAsync(Guid id)
    {
        return await _context
            .Empresas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

}
