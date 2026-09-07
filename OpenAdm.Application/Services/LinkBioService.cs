using OpenAdm.Application.Dtos.LinksBio;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Models.LinksBio;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public sealed class LinkBioService(ILinkBioRepository repository, IParceiroRepository parceiroRepository,
    IUsuarioAutenticado usuarioAutenticado, IParceiroAutenticado parceiroAutenticado,
    IUploadImageBlobClient imageUpload) : ILinkBioService
{
    private const string ConfiguracaoNaoEncontrada = "Não foi possível localizar a configuração do link da bio";
    private const string LinkNaoEncontrado = "Não foi possível localizar o link";

    public async Task<ResultPartner<LinkBioConfiguracaoViewModel>> ObterConfiguracaoAsync()
    {
        var configuracao = await repository.ObterConfiguracaoAsync(usuarioAutenticado.ParceiroId);
        if (configuracao == null)
            return (ResultPartner<LinkBioConfiguracaoViewModel>)ConfiguracaoNaoEncontrada;

        return (ResultPartner<LinkBioConfiguracaoViewModel>)Mapear(configuracao);
    }

    public async Task<ResultPartner<LinkBioConfiguracaoViewModel>> CriarOuEditarConfiguracaoAsync(LinkBioConfiguracaoDto dto)
    {
        var erro = ValidarTitulo(dto.Titulo);
        if (erro != null) return (ResultPartner<LinkBioConfiguracaoViewModel>)erro;

        var configuracao = await repository.ObterConfiguracaoAsync(usuarioAutenticado.ParceiroId, true);
        var (backgroundImage, nomeNovoBlob) = await ObterBackgroundImageAsync(dto.BackgroundImage);
        var imagemAlterada = configuracao?.BackgroundImage != backgroundImage;

        if (configuracao != null && imagemAlterada && !string.IsNullOrWhiteSpace(configuracao.NomeBackgroundImage))
        {
            if (!await imageUpload.DeleteImageAsync(configuracao.NomeBackgroundImage))
            {
                if (nomeNovoBlob != null) await imageUpload.DeleteImageAsync(nomeNovoBlob);
                return (ResultPartner<LinkBioConfiguracaoViewModel>)
                    "Não foi possível excluir a imagem anterior";
            }
        }

        if (configuracao == null)
        {
            var data = DateTime.UtcNow;
            configuracao = new LinkBioConfiguracao(Guid.NewGuid(), data, data, 0,
                usuarioAutenticado.ParceiroId, dto.Titulo.Trim(), dto.Descricao?.Trim(),
                dto.CorDeFundo?.Trim(), dto.CorPrincipal?.Trim(), backgroundImage, nomeNovoBlob, dto.Ativo);
            await repository.AdicionarConfiguracaoAsync(configuracao);
        }
        else
        {
            configuracao.Atualizar(dto.Titulo.Trim(), dto.Descricao?.Trim(),
                dto.CorDeFundo?.Trim(), dto.CorPrincipal?.Trim(), backgroundImage,
                imagemAlterada ? nomeNovoBlob : configuracao.NomeBackgroundImage, dto.Ativo);
            repository.AtualizarConfiguracao(configuracao);
        }
        await repository.SaveChangesAsync();
        return (ResultPartner<LinkBioConfiguracaoViewModel>)Mapear(configuracao);
    }

    public async Task<ResultPartner<LinkBioItemViewModel>> CriarLinkAsync(LinkBioItemCreateDto dto)
    {
        var erro = ValidarLink(dto.Titulo, dto.Url, dto.Ordem);
        if (erro != null) return (ResultPartner<LinkBioItemViewModel>)erro;

        var configuracao = await repository.ObterConfiguracaoAsync(usuarioAutenticado.ParceiroId);
        if (configuracao == null) return (ResultPartner<LinkBioItemViewModel>)ConfiguracaoNaoEncontrada;
        var data = DateTime.UtcNow;
        var link = new LinkBioItem(Guid.NewGuid(), data, data, 0, configuracao.Id,
            dto.Titulo.Trim(), dto.Url.Trim(), NormalizarIcone(dto.Icone), dto.Ordem, dto.Ativo);
        await repository.AdicionarLinkAsync(link);
        await repository.SaveChangesAsync();
        return (ResultPartner<LinkBioItemViewModel>)Mapear(link);
    }

    public async Task<ResultPartner<LinkBioItemViewModel>> EditarLinkAsync(LinkBioItemUpdateDto dto)
    {
        var erro = ValidarLink(dto.Titulo, dto.Url, dto.Ordem);
        if (erro != null) return (ResultPartner<LinkBioItemViewModel>)erro;

        var link = await repository.ObterLinkAsync(dto.Id, usuarioAutenticado.ParceiroId);
        if (link == null) return (ResultPartner<LinkBioItemViewModel>)LinkNaoEncontrado;
        link.Atualizar(dto.Titulo.Trim(), dto.Url.Trim(), NormalizarIcone(dto.Icone), dto.Ordem, dto.Ativo);
        repository.AtualizarLink(link);
        await repository.SaveChangesAsync();
        return (ResultPartner<LinkBioItemViewModel>)Mapear(link);
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> ExcluirLinkAsync(Guid id)
    {
        var link = await repository.ObterLinkAsync(id, usuarioAutenticado.ParceiroId);
        if (link == null) return (ResultPartner<ResultadoPadraoViewModel>)LinkNaoEncontrado;
        repository.ExcluirLink(link);
        await repository.SaveChangesAsync();
        return ResultadoPadrao();
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> AlterarStatusAsync(Guid id, bool ativo)
    {
        var link = await repository.ObterLinkAsync(id, usuarioAutenticado.ParceiroId);
        if (link == null) return (ResultPartner<ResultadoPadraoViewModel>)LinkNaoEncontrado;
        link.AlterarStatus(ativo);
        repository.AtualizarLink(link);
        await repository.SaveChangesAsync();
        return ResultadoPadrao();
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> AlterarOrdemAsync(LinkBioAlterarOrdemDto dto)
    {
        var erro = ValidarOrdem(dto.Ordem);
        if (erro != null) return (ResultPartner<ResultadoPadraoViewModel>)erro;

        var link = await repository.ObterLinkAsync(dto.Id, usuarioAutenticado.ParceiroId);
        if (link == null) return (ResultPartner<ResultadoPadraoViewModel>)LinkNaoEncontrado;
        link.AlterarOrdem(dto.Ordem);
        repository.AtualizarLink(link);
        await repository.SaveChangesAsync();
        return ResultadoPadrao();
    }

    public async Task<ResultPartner<LinkBioPaginaPublicaViewModel>> ObterPaginaPublicaAsync()
    {
        var configuracao = await repository.ObterPaginaPublicaAsync(parceiroAutenticado.Id);
        if (configuracao == null)
            return (ResultPartner<LinkBioPaginaPublicaViewModel>)ConfiguracaoNaoEncontrada;

        var parceiro = await parceiroRepository.ObterPorEmpresaOpenAdmIdAsync(configuracao.EmpresaId);
        if (parceiro == null)
            return (ResultPartner<LinkBioPaginaPublicaViewModel>)"Não foi possível localizar a empresa";

        await RegistrarEventoAsync(configuracao, null, TipoEventoLinkBioEnum.Visualizacao);
        return (ResultPartner<LinkBioPaginaPublicaViewModel>)new LinkBioPaginaPublicaViewModel
        {
            NomeFantasia = parceiro.NomeFantasia,
            Logo = parceiro.Logo.ParaString(),
            Titulo = configuracao.Titulo,
            Descricao = configuracao.Descricao,
            CorDeFundo = configuracao.CorDeFundo,
            CorPrincipal = configuracao.CorPrincipal,
            BackgroundImage = configuracao.BackgroundImage,
            Links = configuracao.Links.Where(x => x.Ativo).OrderBy(x => x.Ordem).ThenBy(x => x.Id)
                .Select(x => new LinkBioItemPublicoViewModel
                    { Id = x.Id, Titulo = x.Titulo, Url = x.Url, Icone = x.Icone, Ordem = x.Ordem }).ToList()
        };
    }

    public async Task<ResultPartner<ResultadoPadraoViewModel>> RegistrarCliqueAsync(Guid linkId)
    {
        var configuracao = await repository.ObterPaginaPublicaAsync(parceiroAutenticado.Id);
        if (configuracao == null) return (ResultPartner<ResultadoPadraoViewModel>)ConfiguracaoNaoEncontrada;

        var link = await repository.ObterLinkPublicoAsync(linkId, configuracao.Id);
        if (link == null) return (ResultPartner<ResultadoPadraoViewModel>)LinkNaoEncontrado;

        await RegistrarEventoAsync(configuracao, link.Id, TipoEventoLinkBioEnum.Clique);
        return ResultadoPadrao();
    }

    public async Task<ResultPartner<LinkBioIndicadoresViewModel>> ConsultarEventosAsync(LinkBioEventosFiltroDto filtro)
    {
        if (filtro.DataInicial == default || filtro.DataFinal == default || filtro.DataInicial > filtro.DataFinal)
            return (ResultPartner<LinkBioIndicadoresViewModel>)"Informe um período válido";
        var resultado = await repository.ConsultarEventosAsync(usuarioAutenticado.ParceiroId,
            filtro.DataInicial, filtro.DataFinal);
        return (ResultPartner<LinkBioIndicadoresViewModel>)new LinkBioIndicadoresViewModel
        {
            Visualizacoes = resultado.Visualizacoes,
            Cliques = resultado.Cliques,
            Eventos = resultado.Eventos.Select(x => new LinkBioEventoViewModel
                { Id = x.Id, LinkId = x.LinkBioItemId, Tipo = x.Tipo, DataHora = x.DataDeCriacao }).ToList(),
            LinksMaisClicados = resultado.LinksMaisClicados.Select(x => new LinkBioMaisClicadoViewModel
                { LinkId = x.LinkId, Titulo = x.Titulo, Quantidade = x.Quantidade }).ToList()
        };
    }

    private async Task RegistrarEventoAsync(LinkBioConfiguracao configuracao, Guid? linkId,
        TipoEventoLinkBioEnum tipo)
    {
        var data = DateTime.UtcNow;
        await repository.AdicionarEventoAsync(new LinkBioEvento(Guid.NewGuid(), data, data, 0,
            configuracao.EmpresaId, configuracao.Id, linkId, tipo));
        await repository.SaveChangesAsync();
    }

    private static string? ValidarTitulo(string titulo) =>
        string.IsNullOrWhiteSpace(titulo) ? "Informe o título" : null;

    private static string? ValidarLink(string titulo, string url, int ordem)
    {
        var erro = ValidarTitulo(titulo);
        if (erro != null) return erro;

        if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            return "Informe uma URL válida";

        return ValidarOrdem(ordem);
    }

    private static string? ValidarOrdem(int ordem) => ordem < 0 ? "Informe uma ordem válida" : null;

    private static ResultPartner<ResultadoPadraoViewModel> ResultadoPadrao() =>
        (ResultPartner<ResultadoPadraoViewModel>)new ResultadoPadraoViewModel { Resultado = true };

    private static string? NormalizarIcone(string? icone) =>
        string.IsNullOrWhiteSpace(icone) ? null : icone.Trim();

    private async Task<(string? Url, string? NomeBlob)> ObterBackgroundImageAsync(string? backgroundImage)
    {
        if (string.IsNullOrWhiteSpace(backgroundImage)) return (null, null);

        var conteudo = backgroundImage.Trim();
        if (conteudo.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) return (conteudo, null);

        if (conteudo.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
        {
            var separador = conteudo.IndexOf(',');
            if (separador >= 0) conteudo = conteudo[(separador + 1)..];
        }

        var nomeBlob = $"{Guid.NewGuid()}.jpeg";
        var url = await imageUpload.UploadImageAsync(conteudo, nomeBlob);
        return (url, nomeBlob);
    }

    private static LinkBioConfiguracaoViewModel Mapear(LinkBioConfiguracao x) => new()
    {
        Id = x.Id, Titulo = x.Titulo, Descricao = x.Descricao,
        CorDeFundo = x.CorDeFundo, CorPrincipal = x.CorPrincipal,
        BackgroundImage = x.BackgroundImage, Ativo = x.Ativo,
        Links = x.Links.OrderBy(link => link.Ordem).ThenBy(link => link.Id).Select(Mapear).ToList()
    };

    private static LinkBioItemViewModel Mapear(LinkBioItem x) => new()
    {
        Id = x.Id, Titulo = x.Titulo, Url = x.Url, Icone = x.Icone, Ordem = x.Ordem, Ativo = x.Ativo
    };
}
