using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Services;

public class TokenService : ITokenService
{
    private const string KeySessaoId = "SessaoId";
    private const string KeyUsuarioId = "UsuarioId";
    private const string KeyParceiroId = "ParceiroId";
    private const string KeyTipoUsuario = "TipoUsuario";
    private const string KeyEhFuncionarioLegado = "EhFuncionario";
    private const string KeyDataLogin = "DataLogin";

    public string GenerateToken(SessaoUsuario sessao)
    {
        return Generate(sessao, DateTime.UtcNow.AddHours(ConfiguracaoDeToken.Expiration));
    }

    private static string Generate(SessaoUsuario sessao, DateTime expires)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(ConfiguracaoDeToken.Key));

        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: ConfiguracaoDeToken.Issue,
            audience: ConfiguracaoDeToken.Audience,
            claims: GenerateClaims(sessao),
            expires: expires,
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static Claim[] GenerateClaims(SessaoUsuario sessao)
    {
        var claims = new List<Claim>()
        {
            new(KeySessaoId, sessao.Id.ToString()),
            new(KeyUsuarioId, sessao.UsuarioId.ToString()),
            new(KeyParceiroId, sessao.ParceiroId.ToString()),
            new(KeyTipoUsuario, sessao.TipoUsuario.ToString()),
            new(KeyDataLogin, sessao.DataDeCriacao.FormatarDataJson()),
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

            var sessaoId = jwtToken.Claims.FirstOrDefault(c => c.Type == KeySessaoId)?.Value;
            var usuarioId = jwtToken.Claims.FirstOrDefault(c => c.Type == KeyUsuarioId)?.Value;
            var parceiroId = jwtToken.Claims.FirstOrDefault(c => c.Type == KeyParceiroId)?.Value;
            var tipoUsuarioClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == KeyTipoUsuario)?.Value;
            var ehFuncionarioLegado = jwtToken.Claims.FirstOrDefault(c => c.Type == KeyEhFuncionarioLegado)?.Value;
            var dataLogin = jwtToken.Claims.FirstOrDefault(c => c.Type == KeyDataLogin)?.Value;

            if (!Guid.TryParse(sessaoId, out var sessaoIdParse) ||
                !Guid.TryParse(usuarioId, out var usuarioIdParse) ||
                !Guid.TryParse(parceiroId, out var parceiroIdParse) ||
                !DateTime.TryParse(dataLogin, out var dataLoginParse) ||
                !TryObterTipoUsuario(tipoUsuarioClaim, ehFuncionarioLegado, out var tipoUsuario))
            {
                return (ResultPartner<ValidaTokenModel>)"JWT inválido, efetue o login";
            }

            return (ResultPartner<ValidaTokenModel>)new ValidaTokenModel()
            {
                Expirado = false,
                TipoUsuario = tipoUsuario,
                Id = usuarioIdParse,
                ParceiroId = parceiroIdParse,
                SessaoId = sessaoIdParse,
                DataDoLogin = dataLoginParse
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

    private static bool TryObterTipoUsuario(
        string? tipoUsuarioClaim,
        string? ehFuncionarioLegado,
        out TipoUsuario tipoUsuario)
    {
        if (Enum.TryParse(tipoUsuarioClaim, ignoreCase: false, out tipoUsuario) &&
            Enum.IsDefined(tipoUsuario))
        {
            return true;
        }

        tipoUsuario = ehFuncionarioLegado switch
        {
            "TRUE" => TipoUsuario.Funcionario,
            "FALSE" => TipoUsuario.Usuario,
            _ => default
        };

        return ehFuncionarioLegado is "TRUE" or "FALSE";
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
