using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Domain.Model;

namespace OpenAdm.Api.Extensions;

public static class ControllerExtension
{
    public static IActionResult ToActionResult<T>(this ResultPartner<T> result)
    {
        if (!string.IsNullOrWhiteSpace(result.Error))
        {
            return new BadRequestObjectResult(new ErrorResponse()
            {
                Mensagem = result.Error,
            });
        }

        return new OkObjectResult(result.Result);
    }
}