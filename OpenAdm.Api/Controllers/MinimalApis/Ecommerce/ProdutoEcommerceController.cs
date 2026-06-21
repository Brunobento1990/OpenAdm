using OpenAdm.Api.Attributes;
using OpenAdm.Api.Configure;
using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Interfaces.Ecommerce;

namespace OpenAdm.Api.Controllers.MinimalApis.Ecommerce;

public class ProdutoEcommerceController : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/produto-ecommerce");

        group.MapPost(
                "/",
                async (
                    IProdutoEcommerceService service,
                    ProdutoEcommerceFiltroDto dto
                ) =>
                {
                    var response =
                        await service.ListarAsync(dto);

                    return Results.Ok(response);
                })
            .WithMetadata(new TryAutenticaAttribute())
            .WithMetadata(new AcessoParceiroAttribute());
    }
}