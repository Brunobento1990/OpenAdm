using OpenAdm.Api.Midlewares;

namespace OpenAdm.Api.Middlewares;

public static class InjectMiddleware
{

    public static void AddMiddlewaresApi(this WebApplication app)
    {
        app.UseMiddleware<LogMiddleware>();
        app.UseMiddleware<ParceiroMiddleware>();
        app.UseMiddleware<AuthorizeMiddleware>();
        app.UseMiddleware<LoginFuncionarioMiddleware>();
    }
}
