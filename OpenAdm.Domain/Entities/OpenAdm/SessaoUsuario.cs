using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Domain.Entities.OpenAdm;

public class SessaoUsuario : BaseEntity
{
    private SessaoUsuario(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        Guid usuarioId,
        Guid parceiroId,
        bool ehFuncionario,
        DateTime? ultimaAtividadeEm,
        DateTime expiraEm, DateTime? revogadoEm, string? enderecoIp, string? userAgent, string? sistemaOperacional,
        string? navegador, string? dispositivo)
        : base(id, dataDeCriacao, dataDeAtualizacao, 0)
    {
        UsuarioId = usuarioId;
        ParceiroId = parceiroId;
        EhFuncionario = ehFuncionario;
        UltimaAtividadeEm = ultimaAtividadeEm;
        ExpiraEm = expiraEm;
        RevogadoEm = revogadoEm;
        EnderecoIp = enderecoIp;
        UserAgent = userAgent;
        SistemaOperacional = sistemaOperacional;
        Navegador = navegador;
        Dispositivo = dispositivo;
    }

    public Guid UsuarioId { get; private set; }
    public Guid ParceiroId { get; private set; }
    public bool EhFuncionario { get; private set; }
    public DateTime? UltimaAtividadeEm { get; private set; }
    public DateTime ExpiraEm { get; private set; }
    public DateTime? RevogadoEm { get; private set; }
    public string? EnderecoIp { get; private set; }
    public string? UserAgent { get; private set; }
    public string? SistemaOperacional { get; private set; }
    public string? Navegador { get; private set; }
    public string? Dispositivo { get; private set; }

    public bool Ativa =>
        RevogadoEm == null &&
        ExpiraEm > DateTime.UtcNow;

    public static SessaoUsuario Criar(
        Guid usuarioId,
        Guid parceiroId,
        bool ehFuncionario,
        int expiracaoEmDias,
        IUsuarioSessaoRequest usuarioSessaoRequest)
    {
        var agora = DateTime.UtcNow;

        return new SessaoUsuario(
            Guid.NewGuid(), agora, agora, usuarioId, parceiroId, ehFuncionario, agora,
            agora.AddDays(expiracaoEmDias), null,
            usuarioSessaoRequest.EnderecoIp?.Limitar(SessaoUsuarioConfig.MaxLengthEnderecoIp),
            usuarioSessaoRequest.UserAgent?.Limitar(SessaoUsuarioConfig.MaxLengthUserAgent),
            usuarioSessaoRequest.SistemaOperacional?.Limitar(SessaoUsuarioConfig.MaxLengthSistemaOperacional),
            usuarioSessaoRequest.Navegador?.Limitar(SessaoUsuarioConfig.MaxLengthNavegador),
            usuarioSessaoRequest.Dispositivo?.Limitar(SessaoUsuarioConfig.MaxLengthDispositivo));
    }
}

public static class SessaoUsuarioConfig
{
    public const int MaxLengthEnderecoIp = 100;
    public const int MaxLengthUserAgent = 1000;
    public const int MaxLengthSistemaOperacional = 1000;
    public const int MaxLengthNavegador = 1000;
    public const int MaxLengthDispositivo = 1000;
    public const string ErroSessaoNaoEncontrada = "Sessão não encontrada.";
}
