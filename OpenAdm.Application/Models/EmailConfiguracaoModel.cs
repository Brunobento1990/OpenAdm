using OpenAdm.Domain.Helpers;

namespace OpenAdm.Application.Models;

public static class EmailConfiguracaoModel
{
    public static int Porta;
    public static string Email = string.Empty;
    public static string Servidor = string.Empty;
    private static string _senha = string.Empty;

    public static void Configure(string email, string servidor, string senha, int porta)
    {
        Email = email;
        Servidor = servidor;
        _senha = Criptografia.Encrypt(senha);
        Porta = porta;
    }

    public static string Senha => Criptografia.Decrypt(_senha);
}
