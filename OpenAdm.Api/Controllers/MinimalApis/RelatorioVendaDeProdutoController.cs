using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Model;

namespace OpenAdm.Api.Controllers.MinimalApis;

public static class RelatorioVendaDeProdutoController
{
    public static WebApplication MaperControllerRelatorioVendaDeProduto(this WebApplication app)
    {
        app.MapPost("relatorio-venda-produto",
                async ([FromBody] RelatorioVendaDeProdutoDTO dto,
                    [FromServices] IRelatorioVendaDeProdutoService service) =>
                {
                    var resultado = await service.ListarAsync(dto);
                    return Results.Ok(resultado);
                })
            .WithMetadata(new AcessoParceiroAttribute())
            .WithMetadata(new AutenticaAttribute())
            .WithMetadata(new IsFuncionarioAttribute())
            .Produces<PaginacaoViewModel<RelatorioVendaDeProdutoViewModel>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        return app;
    }
}