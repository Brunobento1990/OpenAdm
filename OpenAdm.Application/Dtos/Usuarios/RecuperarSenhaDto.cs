using OpenAdm.Application.Adapters;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Extensions;

namespace OpenAdm.Application.Dtos.Usuarios;

public class RecuperarSenhaDto
{
    public string Senha { get; set; } = string.Empty;
    public string ReSenha { get; set; } = string.Empty;
    public Guid TokenEsqueceuSenha { get; set; }

    public void Validar()
    {
        Senha.ValidarNullOrEmpty("Informe a senha").ValidarLength(255);
        ReSenha.ValidarNullOrEmpty("Confirme a senha").ValidarLength(255);

        if (!Senha.Equals(ReSenha))
        {
            throw new ExceptionApi("As senha não conferem");
        }
    }

    public string HashSenha()
    {
        return PasswordAdapter.GenerateHash(Senha);
    }
}
