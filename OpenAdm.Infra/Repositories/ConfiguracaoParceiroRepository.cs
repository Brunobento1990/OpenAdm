using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracaoParceiroRepository
    : IConfiguracaoParceiroRepository
{
    private readonly OpenAdmContext _context;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    public ConfiguracaoParceiroRepository(
        OpenAdmContext context, IParceiroAutenticado parceiroAutenticado)
    {
        _context = context;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<ConfiguracaoParceiro?> GetParceiroAutenticadoAdmAsync()
    {
        var keyParceiro = Guid.Parse(_parceiroAutenticado.KeyParceiro);
        return await _context
            .ConfiguracoesParceiro
            .Include(x => x.Parceiro)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ParceiroId == keyParceiro);
    }

    public async Task<ConfiguracaoParceiro?> GetParceiroByDominioAdmAsync(string dominio)
    {
        return await _context
            .ConfiguracoesParceiro
            .Include(x => x.Parceiro)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DominioSiteAdm == dominio || x.DominioSiteEcommerce == dominio);
    }
}
