namespace OpenAdm.Infra.HttpService.Interfaces;

public interface IDiscordHttpService
{
    Task NotifyExceptionAsync(string message, string webHookId, string webHookToken);
}
