using Microsoft.AspNetCore.Mvc;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.HttpService.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
public abstract class ControllerBaseApi : ControllerBase
{
    protected async Task<IActionResult> HandleErrorAsync(Exception ex)
    {
        if (ex.GetType() == typeof(ExceptionApi) || VariaveisDeAmbiente.GetVariavel("AMBIENTE").Equals("develop"))
            return BadRequest(new { ex.Message });

        var discordHttpService = HttpContext.RequestServices.GetRequiredService<IDiscordHttpService>();

        var webHookId = VariaveisDeAmbiente.GetVariavel("DISCORD_WEB_HOOK_ID");
        var webHookToken = VariaveisDeAmbiente.GetVariavel("DISCORD_WEB_HOOK_TOKEN");

        await discordHttpService.NotifyExceptionAsync(ex.Message, webHookId, webHookToken);

        return BadRequest(new { Message = "Ocorreu um erro interno, tente novamente mais tarde, ou entre em contato com o suporte!" });
    }
}
