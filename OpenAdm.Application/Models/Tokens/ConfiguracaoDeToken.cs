using OpenAdm.Application.Models.Funcionarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OpenAdm.Application.Models.Tokens;

public static class ConfiguracaoDeToken
{
    public static string Key { get; private set; } = string.Empty;
    public static string Issue { get; private set; } = string.Empty;
    public static string Audience { get; private set; } = string.Empty;
    public static int Expiration { get; private set; }

    public static void Configure(string key, string issue, string audience, int expiration)
    {
        Key = key;
        Issue = issue;
        Audience = audience;
        Expiration = expiration;
    }

    public static Claim[] GenerateClaimsFuncionario(object obj)
    {
        var claims = new List<Claim>();

        foreach (var property in obj.GetType().GetProperties())
        {
            var value = property.GetValue(obj);
            if (value != null)
                claims.Add(new Claim(property.Name, value.ToString() ?? "Sem Valor"));

        }

        var isFuncionario = obj.GetType() == typeof(FuncionarioViewModel);

        if (isFuncionario)
            claims.Add(new Claim("IsFuncionario", "TRUE"));

        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        return [.. claims];
    }
}
