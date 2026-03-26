using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Infra.Model;
using Serilog;

namespace OpenAdm.Api.Extensions;

public static class HttpContextExtension
{
    public static bool TemAtributo<T>(this HttpContext httpContext)
    {
        return httpContext.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata
            .OfType<T>()
            .Any() ?? false;
    }

    public static async Task RetornarErroAsync(this HttpContext context, string erro,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var errorResponse = new ErrorResponse()
        {
            Mensagem = erro
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)httpStatusCode;
        var result = JsonSerializer.Serialize(errorResponse, JsonSerializerOptionsApi.Options());
        Log.Error(result, "Body response");
        await context.Response.WriteAsync(result);
    }
}