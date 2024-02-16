using OpenAdm.Infra.HttpService.Services;

namespace OpenAdm.Test.Infra.Test;

public class DiscordHttpServiceTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveNotificarDiscordSemMensage(string message)
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var discordHttpService = new DiscordHttpService(httpClientFactory.Object);

        await Assert.ThrowsAnyAsync<Exception>(
            async () => await discordHttpService.NotifyExceptionAsync(message, "", ""));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveNotificarDiscordSemWebHookId(string webHookId)
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var discordHttpService = new DiscordHttpService(httpClientFactory.Object);

        await Assert.ThrowsAnyAsync<Exception>(
            async () => await discordHttpService.NotifyExceptionAsync("Teste", webHookId, ""));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveNotificarDiscordSemWebHookToken(string webHookToken)
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var discordHttpService = new DiscordHttpService(httpClientFactory.Object);

        await Assert.ThrowsAnyAsync<Exception>(
            async () => await discordHttpService.NotifyExceptionAsync("Teste", "123", webHookToken));
    }
}
