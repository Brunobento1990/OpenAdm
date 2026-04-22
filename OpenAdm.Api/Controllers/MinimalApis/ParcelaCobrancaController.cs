using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Api.Controllers.MinimalApis;

public static class ParcelaCobrancaController
{
    public static WebApplication MaperControllerParcelaCobranca(this WebApplication app)
    {
        var group = app.MapGroup("parcela-cobranca");

        group.MapPost("paginacao",
                async (IParcelaCobrancaService service, [FromBody] PaginacaoParcelaCobrancaDTO dto) =>
                {
                    var response = await service.PaginacaoAsync(dto);
                    return Results.Ok(response);
                })
            .WithMetadata(new AcessoParceiroAttribute())
            .WithMetadata(new AutenticaAttribute())
            .WithMetadata(new IsFuncionarioAttribute())
            .Produces<PaginacaoViewModel<ParcelaCobrancaViewModel>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        return app;
    }
}