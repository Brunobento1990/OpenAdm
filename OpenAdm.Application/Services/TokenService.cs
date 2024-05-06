using Domain.Pkg.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Funcionarios;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Models.Usuarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenAdm.Application.Services;

public class TokenService(IHttpContextAccessor httpContextAccessor) : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GenerateRefreshToken(Guid id)
    {
        return Genereate(new { Id = id }, DateTime.Now.AddDays(30));
    }
    public string GenerateToken(object obj)
    {
        return Genereate(obj, DateTime.Now.AddHours(ConfiguracaoDeToken.Expiration));
    }
    public UsuarioViewModel GetTokenUsuarioViewModel()
    {
        if (_httpContextAccessor?.HttpContext?.User.Identity is not ClaimsIdentity claimsIdentity
            || !claimsIdentity.Claims.Any())
            throw new ExceptionApi(nameof(claimsIdentity));

        var id = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value
            ?? throw new ExceptionApi("token inválido");
        var numero = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Numero")?.Value
            ?? throw new ExceptionApi("token inválido");
        var nome = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Nome")?.Value
            ?? throw new ExceptionApi("token inválido");
        var email = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value
            ?? throw new ExceptionApi("token inválido");

        var dataDeCriacao = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "DataDeCriacao")?.Value
            ?? throw new ExceptionApi("token inválido");

        var dataDeAtualizacao = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "DataDeAtualizacao")?.Value
            ?? throw new ExceptionApi("token inválido");

        var telefone = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Telefone")?.Value;
        var cnpj = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
        var cpf = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Cpf")?.Value;

        if (!DateTime.TryParse(dataDeCriacao, out DateTime newDataDeCriacao))
            throw new ExceptionApi("token inválido");

        if (!DateTime.TryParse(dataDeAtualizacao, out DateTime newDataDeAtualizacao))
            throw new ExceptionApi("token inválido");

        return new UsuarioViewModel()
        {
            Id = Guid.Parse(id),
            Nome = nome,
            Email = email,
            Numero = long.Parse(numero),
            DataDeCriacao = newDataDeCriacao,
            DataDeAtualizacao = newDataDeAtualizacao,
            Telefone = telefone,
            Cnpj = cnpj,
            Cpf = cpf
        };
    }
    public bool IsFuncionario()
    {
        if (_httpContextAccessor?.HttpContext?.User.Identity is not ClaimsIdentity claimsIdentity
            || !claimsIdentity.Claims.Any())
            throw new ExceptionApi(nameof(claimsIdentity));

        var isFuncionario = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "IsFuncionario")?.Value;

        return !string.IsNullOrWhiteSpace(isFuncionario) && isFuncionario == "TRUE";
    }
    private static string Genereate(object obj, DateTime expires)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(ConfiguracaoDeToken.Key));

        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
          issuer: ConfiguracaoDeToken.Issue,
          audience: ConfiguracaoDeToken.Audience,
          claims: GenerateClaims(obj),
          expires: expires,
          signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public static Claim[] GenerateClaims(object obj)
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
