using Microsoft.AspNetCore.Http;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Domain.Factories.Interfaces;

namespace OpenAdm.Infra.Factories.Factory;

public class DomainFactory : IDomainFactory
{
    private readonly string? _dominio;
    public DomainFactory(IHttpContextAccessor httpContextAccessor)
    {
        _dominio = httpContextAccessor?
           .HttpContext?
           .Request?
           .Headers?
           .FirstOrDefault(x => x.Key == "Referer").Value.ToString();
    }
    public string GetDomainParceiro()
    {
        if (string.IsNullOrWhiteSpace(_dominio))
            throw new ExceptionApi(CodigoErrors.OrigemDaRequisicaoInvalida);

        return _dominio;
    }
}
