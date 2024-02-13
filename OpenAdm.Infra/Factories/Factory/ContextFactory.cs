using Microsoft.AspNetCore.Http;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Factories.Interfaces;

namespace OpenAdm.Infra.Factories.Factory;

public class ContextFactory : IContextFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContextFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Task<ParceiroContext> GetParceiroContextAsync()
    {
        var context = _httpContextAccessor.HttpContext.Items["ParceiroContext"] as ParceiroContext
            ?? throw new ExceptionApi(ContextErrorMessage.ContextInjetadoIncorretamente);

        return Task.FromResult(context);
    }
}
