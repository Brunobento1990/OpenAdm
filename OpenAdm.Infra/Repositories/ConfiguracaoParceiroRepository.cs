using Domain.Pkg.Cryptography;
using Domain.Pkg.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ConfiguracaoParceiroRepository
    : IConfiguracaoParceiroRepository
{
    private readonly OpenAdmContext _context;
    private readonly string _dominio;

    public ConfiguracaoParceiroRepository(
        OpenAdmContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _dominio = httpContextAccessor?
           .HttpContext?
           .Request?
           .Headers?
           .FirstOrDefault(x => x.Key == "Referer").Value.ToString() ?? throw new Exception();
    }

    public async Task<string> GetConexaoDbByDominioAsync()
    {
        var encrypt = await _context
            .ConfiguracoesParceiro
            .AsNoTracking()
            .Where(x => x.Ativo && (x.DominioSiteAdm == _dominio || x.DominioSiteEcommerce == _dominio))
            .Select(x => x.ConexaoDb)
            .FirstOrDefaultAsync()
                ?? throw new ExceptionApi();

        return CryptographyGeneric.Decrypt(encrypt);
    }
}
