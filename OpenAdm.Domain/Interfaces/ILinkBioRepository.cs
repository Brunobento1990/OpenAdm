using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Interfaces;

public interface ILinkBioRepository
{
    Task<LinkBioConfiguracao?> ObterConfiguracaoAsync(Guid empresaId, bool rastrear = false);
    Task<LinkBioConfiguracao?> ObterPaginaPublicaAsync(Guid empresaId);
    Task<LinkBioItem?> ObterLinkAsync(Guid id, Guid empresaId);
    Task<LinkBioItem?> ObterLinkPublicoAsync(Guid id, Guid configuracaoId);
    Task AdicionarConfiguracaoAsync(LinkBioConfiguracao configuracao);
    Task AdicionarLinkAsync(LinkBioItem link);
    Task AdicionarEventoAsync(LinkBioEvento evento);
    void AtualizarConfiguracao(LinkBioConfiguracao configuracao);
    void AtualizarLink(LinkBioItem link);
    void ExcluirLink(LinkBioItem link);
    Task<(int Visualizacoes, int Cliques, IList<LinkBioEvento> Eventos,
        IList<(Guid LinkId, string Titulo, int Quantidade)> LinksMaisClicados)> ConsultarEventosAsync(
        Guid empresaId, DateTime dataInicial, DateTime dataFinal);
    Task SaveChangesAsync();
}
