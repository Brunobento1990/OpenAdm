using Microsoft.IdentityModel.Tokens;
using OpenAdm.Application.Models.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenAdm.Application.Adapters;

public static class JwtAdapter
{
    public static SymmetricSecurityKey GenerateSymmetricSecurityKey(string key)
    {
        return new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(ConfiguracaoDeToken.Key));
    }

    public static JwtSecurityToken GenerateJwtSecurityToken(
        string? issuer = null, 
        string? audience = null, 
        IEnumerable<Claim>? claims = null,
        DateTime? expires = null,
        SigningCredentials? signingCredentials = null)
    {
        return new JwtSecurityToken(
          issuer: issuer,
          audience: audience,
          claims: claims,
          expires: expires,
          signingCredentials: signingCredentials);
    }

    public static string GenerateToken(JwtSecurityToken jwtSecurityToken)
    {
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
