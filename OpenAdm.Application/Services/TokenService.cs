using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenAdm.Application.Services;

public class TokenService : ITokenService
{
    private static string KeyId = "Id";
    private static string KeyIsFuncionario = "Id";

    public string GenerateRefreshToken(Guid id, bool isFuncionario)
    {
        return Genereate(id, isFuncionario, DateTime.Now.AddDays(30));
    }

    public string GenerateToken(Guid id, bool isFuncionario)
    {
        return Genereate(id, isFuncionario, DateTime.Now.AddHours(ConfiguracaoDeToken.Expiration));
    }

    private static string Genereate(Guid id, bool isFuncionario, DateTime expires)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(ConfiguracaoDeToken.Key));

        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: ConfiguracaoDeToken.Issue,
            audience: ConfiguracaoDeToken.Audience,
            claims: GenerateClaims(id, isFuncionario),
            expires: expires,
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static Claim[] GenerateClaims(Guid id, bool isFuncionario)
    {
        var claims = new List<Claim>()
        {
            new Claim(KeyId, id.ToString())
        };

        if (isFuncionario)
        {
            claims.Add(new Claim(KeyIsFuncionario, "TRUE"));
        }

        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        return [.. claims];
    }

    public async Task<TokenResponseGoogleModel> ValidarTokenGoogleAsync(string token)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { ConfiguracaoDeToken.ClientIdGoogle }
                });

            return new TokenResponseGoogleModel()
            {
                Email = payload.Email,
                Foto = payload.Picture
            };
        }
        catch (InvalidJwtException ex)
        {
            throw new ExceptionApi(ex.Message);
        }
    }
}