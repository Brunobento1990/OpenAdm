using OpenAdm.Domain.Errors;
using OpenAdm.Infra.Factories.Interfaces;
using System.Text.Json;

namespace OpenAdm.Api.Middlewares;

public class FactoryContextMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {

            var origin = context.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;

            if (string.IsNullOrWhiteSpace(origin))
            {
                await SetErrorResponseAsync(context, "Você não tem permissão para acessar essa rota!", 401);

                return;
            }

            var contextFactory = context.Request.HttpContext.RequestServices.GetRequiredService<IParceiroContextFactory>();

            var contextParceiro = await contextFactory.CreateParceiroContextAsync();

            var addOk = context.Items.TryAdd("ParceiroContext", contextParceiro);

            if (!addOk)
            {
                await SetErrorResponseAsync(context, ContextErrorMessage.ContextInjetadoIncorretamente, 400);
                return;
            }

            await next(context);
        }
        catch (Exception)
        {
            await SetErrorResponseAsync(context, "Ocorreu um erro interno, tente novamente mais tarde, ou entre em contato com o suporte!", 400);
            return;
        }
    }

    private static async Task SetErrorResponseAsync(HttpContext context, string message, int code)
    {
        context.Response.StatusCode = code;
        context.Response.ContentType = "application/json";
        var responseJson = JsonSerializer.Serialize(new { message });
        await context.Response.WriteAsync(responseJson);
    }
}
