using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Home;

namespace OpenAdm.Api.Controllers.MinimalApis;

public static class HomeController
{
    public static WebApplication MaperControllerHome(this WebApplication app)
    {
        var group = app.MapGroup("home");

        group
            .MapGet("adm",
                async (IHomeSevice service) =>
                {
                    var response = await service.GetHomeAdmAsync();
                    return Results.Ok(response);
                })
            .WithMetadata(new AcessoParceiroAttribute())
            .WithMetadata(new AutenticaAttribute())
            .WithMetadata(new IsFuncionarioAttribute())
            .Produces<HomeAdmViewModel>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        return app;
    }
}