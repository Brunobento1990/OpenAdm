using Microsoft.IdentityModel.Tokens;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenAdm.Application.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ITokenService _tokenService;
    private readonly IUsuarioRepository _usuarioRepository;

    public RefreshTokenService(ITokenService tokenService, IUsuarioRepository usuarioRepository)
    {
        _tokenService = tokenService;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<string> RefreshTokenAsync(string token, string refreshToken)
    {
        if (token.Equals("null") || string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return string.Empty;
        }
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = ConfiguracaoDeToken.Issue,
            ValidAudience = ConfiguracaoDeToken.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfiguracaoDeToken.Key))
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        }
        catch (SecurityTokenExpiredException)
        {
            tokenValidationParameters.ValidateLifetime = false;
            var refreshPrincipal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out _);
            var idString = refreshPrincipal.FindFirstValue("Id");
            if (Guid.TryParse(idString, out Guid id))
            {
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
                if (usuario != null)
                {
                    var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
                    return _tokenService.GenerateToken(usuarioViewModel);
                }
            }
        }
        catch (Exception)
        {
            return string.Empty;
        }
        return string.Empty;
    }
}