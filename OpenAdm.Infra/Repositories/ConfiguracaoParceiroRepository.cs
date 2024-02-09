using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracaoParceiroRepository(OpenAdmContext context)
    : IConfiguracaoParceiroRepository
{
    private readonly OpenAdmContext _context = context;

    public async Task<string?> GetConexaoDbByDominioAsync(string dominio)
    {
        return await _context
            .ConfiguracoesParceiro
            .AsNoTracking()
            .Where(x => x.Ativo && (x.DominioSiteAdm == dominio || x.DominioSiteEcommerce == dominio))
            .Select(x => x.ConexaoDb)
            .FirstOrDefaultAsync();
    }
}
