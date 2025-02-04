using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.Usuarios;

public class AtualizarSenhaUsuarioAdmDto
{
    public Guid UsuarioId { get; set; }
    public string Senha { get; set; } = string.Empty;
    public string ConfirmarSenha { get; set; } = string.Empty;

    public void Validar()
    {
        if (UsuarioId == Guid.Empty)
        {
            throw new ExceptionApi("Id do usuário inválido");
        }

        if (string.IsNullOrWhiteSpace(Senha))
        {
            throw new ExceptionApi("Senha inválida");
        }

        if (string.IsNullOrWhiteSpace(ConfirmarSenha))
        {
            throw new ExceptionApi("Confirme a senha");
        }

        if (!Senha.Equals(ConfirmarSenha))
        {
            throw new ExceptionApi("As senha não conferem");
        }
    }
}
