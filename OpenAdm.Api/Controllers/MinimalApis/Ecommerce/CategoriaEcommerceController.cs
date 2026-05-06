using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces.Ecommerce;
using OpenAdm.Application.Queries;

namespace OpenAdm.Api.Controllers.MinimalApis.Ecommerce;

public static class CategoriaEcommerceController
{
    public static WebApplication MaperControllerCategoriaEcommerce(this WebApplication app)
    {
        var group = app.MapGroup("ecommerce/categorias");

        group
            .MapGet("listar",
                async (ICategoriaEcommerceService service) =>
                {
                    var response = await service.ListarTodasAsync();
                    return Results.Ok(response);
                })
            .WithMetadata(new AcessoParceiroAttribute())
            .Produces<ICollection<CategoriaEcommerceQuery>>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
        
        group
            .MapGet("home",
                async (ICategoriaEcommerceService service) =>
                {
                    var response = await service.ListarHomeAsync();
                    return Results.Ok(response);
                })
            .WithMetadata(new AcessoParceiroAttribute())
            .Produces<ICollection<CategoriaEcommerceHomeQuery>>()
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        return app;
    }
}