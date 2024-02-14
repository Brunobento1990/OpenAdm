﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using System.Text.Json;

namespace OpenAdm.Api.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
public class IsFuncionarioAttribute : Attribute, IAsyncActionFilter
{
    private const int StatusCodeUnauthorize = 401;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var serviceProvider = context.HttpContext.RequestServices;
        var tokenService = serviceProvider.GetRequiredService<ITokenService>();
        if (!tokenService.IsFuncionario())
        {
            SetUnauthorizedResult(context, "Você não tem permissão para acessar essa rota!");
            return;
        }

        await next();
    }

    private static void SetUnauthorizedResult(ActionExecutingContext context, string message)
    {
        context.Result = new ContentResult()
        {
            StatusCode = StatusCodeUnauthorize,
            Content = JsonSerializer.Serialize(new { message, statusCode = StatusCodeUnauthorize })
        };
    }
}
