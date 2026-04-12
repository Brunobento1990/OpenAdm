using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class TokenService : ITokenService
{
    private static string KeyId = "Id";
    private static string KeyDataLogin = "DataLogin";
    private static string KeyIsFuncionario = "EhFuncionario";

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
            new(KeyId, id.ToString()),
            new(KeyDataLogin, DateTime.Now.FormatarDataJson()),
            new(KeyIsFuncionario, isFuncionario ? "TRUE" : "FALSE"),
        };

        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        return [.. claims];
    }

    public ResultPartner<ValidaTokenModel> ValidarToken(string token, bool validaLifeTime = true)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = validaLifeTime,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = ConfiguracaoDeToken.Issue,
                ValidAudience = ConfiguracaoDeToken.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfiguracaoDeToken.Key))
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == KeyId)?.Value;
            var ehFuncionario = jwtToken.Claims.FirstOrDefault(c => c.Type == KeyIsFuncionario)?.Value;
            var dataLogin = jwtToken.Claims.FirstOrDefault(c => c.Type == KeyDataLogin)?.Value;

            if (!Guid.TryParse(id, out Guid idParse) ||
                string.IsNullOrWhiteSpace(ehFuncionario) ||
                !DateTime.TryParse(dataLogin, out DateTime dataLoginParse))
            {
                return (ResultPartner<ValidaTokenModel>)"JWT inválido, efetue o login";
            }

            return (ResultPartner<ValidaTokenModel>)new ValidaTokenModel()
            {
                Expirado = false,
                DataDoLogin = dataLoginParse,
                EhFuncionario = ehFuncionario == "TRUE",
                Id = idParse
            };
        }
        catch (SecurityTokenExpiredException)
        {
            return (ResultPartner<ValidaTokenModel>)new ValidaTokenModel()
            {
                Expirado = true
            };
        }
        catch (Exception)
        {
            return (ResultPartner<ValidaTokenModel>)"Token inválido, efetue o login";
        }
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