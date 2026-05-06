using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces.Ecommerce;
using OpenAdm.Application.Queries;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers.MinimalApis.Ecommerce;

public static class BannerEcommerceController
{
    public static WebApplication MaperControllerBannerEcommerce(this WebApplication app)
    {
        var group = app.MapGroup("ecommerce/banner");

        group
            .MapGet("listar",
                async (IBannerEcommerceService service, IParceiroAutenticado parceiroAutenticado) =>
                {
                    var response = await service.ListarTodosAsync(parceiroAutenticado.Id);
                    return Results.Ok(response);
                })
            .WithMetadata(new AcessoParceiroAttribute())
            .Produces<ICollection<CategoriaEcommerceQuery>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        return app;
    }
}