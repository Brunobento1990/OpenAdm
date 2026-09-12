using OpenAdm.Application.Attributes;

namespace OpenAdm.Application.Dtos.Funcionarios;

public class TrocarSenhaFuncionarioDto : ValidarBaseDTO
{
    [ValidaString(erro: "Informe a senha atual", maxLength: 255,
        erroMaxLength: "A senha atual deve conter no máximo 255 caracteres")]
    public string SenhaAtual { get; set; } = string.Empty;

    [ValidaString(erro: "Informe a nova senha", maxLength: 255,
        erroMaxLength: "A nova senha deve conter no máximo 255 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [ValidaString(erro: "Confirme a nova senha", maxLength: 255,
        erroMaxLength: "A confirmação da senha deve conter no máximo 255 caracteres")]
    public string ConfirmacaoSenha { get; set; } = string.Empty;

    public override string? Validar()
    {
        var erro = base.Validar();

        if (erro != null)
            return erro;

        return Senha != ConfirmacaoSenha ? "As senhas não conferem!" : null;
    }
}
