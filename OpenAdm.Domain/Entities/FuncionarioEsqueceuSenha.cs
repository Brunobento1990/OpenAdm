using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class FuncionarioEsqueceuSenha : BaseEntityParceiro
{
    public FuncionarioEsqueceuSenha(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        Guid funcionarioId,
        Guid token,
        DateTime dataHoraExpiracao,
        bool resetado,
        Guid parceiroId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero, parceiroId)
    {
        FuncionarioId = funcionarioId;
        Token = token;
        DataHoraExpiracao = dataHoraExpiracao;
        Resetado = resetado;
    }

    public Guid FuncionarioId { get; private set; }
    public Funcionario Funcionario { get; private set; } = null!;
    public Guid Token { get; private set; }
    public DateTime DataHoraExpiracao { get; private set; }
    public bool Resetado { get; private set; }

    public string? PodeRecuperarSenha()
    {
        if (!Funcionario.Ativo)
            return "Token de recuperação inválido!";

        if (Resetado)
            return "Este token de recuperação já foi utilizado!";

        if (DataHoraExpiracao <= DateTime.UtcNow)
            return "Token expirado, solicite uma nova recuperação de senha!";

        return null;
    }

    public void MarcarComoResetado()
    {
        Resetado = true;
        DataDeAtualizacao = DateTime.UtcNow;
    }
}
