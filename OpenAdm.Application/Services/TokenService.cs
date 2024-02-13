using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenAdm.Application.Services;

public class TokenService(IHttpContextAccessor httpContextAccessor) : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GenerateToken(object obj, ConfiguracaoDeToken configGenerateToken)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configGenerateToken.Key));

        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
          issuer: configGenerateToken.Issue,
          audience: configGenerateToken.Audience,
          claims: configGenerateToken.GenerateClaimsFuncionario(obj),
          expires: configGenerateToken.Expiration,
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

        return !string.IsNullOrWhiteSpace(isFuncionario) && isFuncionario == "TRUE";
    }
}
