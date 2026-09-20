using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class FuncionarioEsqueceuSenhaBuilder
{
    private Funcionario _funcionario = FuncionarioBuilder.Init().Build();
    private DateTime _expiracao = DateTime.UtcNow.AddHours(1);
    private bool _resetado;

    public static FuncionarioEsqueceuSenhaBuilder Init() => new();

    public FuncionarioEsqueceuSenhaBuilder ComFuncionario(Funcionario funcionario) { _funcionario = funcionario; return this; }
    public FuncionarioEsqueceuSenhaBuilder ComExpiracao(DateTime expiracao) { _expiracao = expiracao; return this; }
    public FuncionarioEsqueceuSenhaBuilder Resetado(bool resetado = true) { _resetado = resetado; return this; }

    public FuncionarioEsqueceuSenha Build()
    {
        var solicitacao = new FuncionarioEsqueceuSenha(
            Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, 1, _funcionario.Id,
            Guid.NewGuid(), _expiracao, _resetado, _funcionario.ParceiroId);
        typeof(FuncionarioEsqueceuSenha)
            .GetProperty(nameof(FuncionarioEsqueceuSenha.Funcionario))!
            .SetValue(solicitacao, _funcionario);
        return solicitacao;
    }
}
