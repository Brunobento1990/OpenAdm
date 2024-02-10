using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenAdm.Application.Services;

public class TokenService(IHttpContextAccessor httpContextAccessor) : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly string _key = "248e9aa8-53a7-4eab-ab98-c2147055e044";
    private readonly string _issuer = "JWT_ISSUER";
    private readonly string _audience = "JWT_AUDIENCE";

    public string GenerateTokenFuncionario(Funcionario funcionario)
    {
        var claims = new[]
        {
            new Claim("Nome", funcionario.Nome),
            new Claim("Email", funcionario.Email),
            new Claim("Numero", funcionario.Numero.ToString()),
            new Claim("Id", funcionario.Id.ToString()),
            new Claim("IsFuncionario", "TRUE"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_key));

        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(24);

        var token = new JwtSecurityToken(
          issuer: _issuer,
          audience: _audience,
          claims: claims,
          expires: expiration,
          signingCredentials: credenciais);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    public bool IsFuncionario()
    {
        if (_httpContextAccessor?.HttpContext?.User.Identity is not ClaimsIdentity claimsIdentity
            || !claimsIdentity.Claims.Any())
            throw new ExceptionApi(nameof(claimsIdentity));

        var isFuncionario = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "IsFuncionario")?.Value;

        return !string.IsNullOrWhiteSpace(isFuncionario);
    }
}
