using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public sealed class LinkBioRepository(AppDbContext appDbContext) : ILinkBioRepository
{
    public Task<LinkBioConfiguracao?> ObterConfiguracaoAsync(Guid empresaId, bool rastrear = false)
    {
        var query = appDbContext.LinkBioConfiguracoes.Include(x => x.Links).AsQueryable();
        
        if (!rastrear)
        {
            query = query.AsNoTracking();
        }
        
        return query.FirstOrDefaultAsync(x => x.EmpresaId == empresaId);
    }

    public Task<LinkBioConfiguracao?> ObterPaginaPublicaAsync(Guid empresaId) => appDbContext.LinkBioConfiguracoes
        .AsNoTracking().Include(x => x.Links)
        .FirstOrDefaultAsync(x => x.EmpresaId == empresaId && x.Ativo);

    public Task<LinkBioItem?> ObterLinkAsync(Guid id, Guid empresaId) => appDbContext.LinkBioItens
        .Include(x => x.LinkBioConfiguracao)
        .FirstOrDefaultAsync(x => x.Id == id && x.LinkBioConfiguracao.EmpresaId == empresaId);

    public Task<LinkBioItem?> ObterLinkPublicoAsync(Guid id, Guid configuracaoId) => appDbContext.LinkBioItens
        .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.LinkBioConfiguracaoId == configuracaoId && x.Ativo);

    public async Task AdicionarConfiguracaoAsync(LinkBioConfiguracao configuracao) =>
        await appDbContext.LinkBioConfiguracoes.AddAsync(configuracao);

    public async Task AdicionarLinkAsync(LinkBioItem link) => await appDbContext.LinkBioItens.AddAsync(link);
    public async Task AdicionarEventoAsync(LinkBioEvento evento) => await appDbContext.LinkBioEventos.AddAsync(evento);
    public void AtualizarConfiguracao(LinkBioConfiguracao configuracao) => appDbContext.LinkBioConfiguracoes.Update(configuracao);
    public void AtualizarLink(LinkBioItem link) => appDbContext.LinkBioItens.Update(link);
    public void ExcluirLink(LinkBioItem link) => appDbContext.LinkBioItens.Remove(link);

    public async Task<(int Visualizacoes, int Cliques, IList<LinkBioEvento> Eventos,
        IList<(Guid LinkId, string Titulo, int Quantidade)> LinksMaisClicados)> ConsultarEventosAsync(
        Guid empresaId, DateTime dataInicial, DateTime dataFinal)
    {
        var query = appDbContext.LinkBioEventos.AsNoTracking()
            .Where(x => x.EmpresaId == empresaId && x.DataDeCriacao >= dataInicial && x.DataDeCriacao <= dataFinal);
        var eventos = await query.OrderByDescending(x => x.DataDeCriacao).ToListAsync();
        var links = await query.Where(x => x.Tipo == TipoEventoLinkBioEnum.Clique && x.LinkBioItemId != null)
            .GroupBy(x => new { LinkId = x.LinkBioItemId!.Value, x.LinkBioItem!.Titulo })
            .Select(x => new { x.Key.LinkId, x.Key.Titulo, Quantidade = x.Count() })
            .OrderByDescending(x => x.Quantidade).ToListAsync();
        return (eventos.Count(x => x.Tipo == TipoEventoLinkBioEnum.Visualizacao),
            eventos.Count(x => x.Tipo == TipoEventoLinkBioEnum.Clique), eventos,
            links.Select(x => (x.LinkId, x.Titulo, x.Quantidade)).ToList());
    }

    public Task SaveChangesAsync() => appDbContext.SaveChangesAsync();
}
