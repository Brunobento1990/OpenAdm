using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Interfaces;

public interface IDiscordHttpService
{
    Task NotifyExceptionAsync(DiscordModel discordModel, string webHookId, string webHookToken);
}
