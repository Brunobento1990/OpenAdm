using Domain.Pkg.Exceptions;
using OpenAdm.Application.Models.Funcionarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OpenAdm.Application.Models.Tokens;

public class ConfiguracaoDeToken(
    string key,
    string issue,
    string audience,
    DateTime expiration)
{
    public string Key { get; private set; } = key ?? throw new ExceptionApi();
    public string Issue { get; private set; } = issue ?? throw new ExceptionApi();
    public string Audience { get; private set; } = audience ?? throw new ExceptionApi();
    public DateTime Expiration { get; private set; } = expiration;

    public Claim[] GenerateClaimsFuncionario(object obj)
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
