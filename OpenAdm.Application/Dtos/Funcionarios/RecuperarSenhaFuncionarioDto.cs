using OpenAdm.Application.Adapters;
using OpenAdm.Application.Attributes;
using OpenAdm.Application.Dtos;

namespace OpenAdm.Application.Dtos.Funcionarios;

public class RecuperarSenhaFuncionarioDto : ValidarBaseDTO
{
    public Guid Token { get; set; }

    [ValidaString(erro: "Informe a senha", maxLength: 255,
        erroMaxLength: "A senha deve conter no máximo 255 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [ValidaString(erro: "Confirme a senha", maxLength: 255,
        erroMaxLength: "A confirmação da senha deve conter no máximo 255 caracteres")]
    public string ConfirmacaoSenha { get; set; } = string.Empty;

    public override string? Validar()
    {
        if (Token == Guid.Empty)
            return "Token de recuperação inválido!";

        var erro = base.Validar();

        if (erro != null)
            return erro;

        return Senha != ConfirmacaoSenha ? "As senhas não conferem!" : null;
    }

    public string HashSenha() => PasswordAdapter.GenerateHash(Senha);
}
