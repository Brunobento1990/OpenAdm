using OpenAdm.Infra.HttpService.Services;
using OpenAdm.Infra.Model;

namespace OpenAdm.Test.Infra.Test;

public class DiscordHttpServiceTest
{

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveNotificarDiscordSemWebHookId(string webHookId)
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var discordHttpService = new DiscordHttpService(httpClientFactory.Object);

        var discordModel = new DiscordModel()
        {
            Content = "Error expeptions",
            Username = "Error",
            Embeds =
            [
                new()
                {
                    Description = "Error",
                    Title = "Error api",
                    Color = 0xFF0000
                }
            ]
        };

        await Assert.ThrowsAnyAsync<Exception>(
            async () => await discordHttpService.NotifyExceptionAsync(discordModel, webHookId, ""));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NaoDeveNotificarDiscordSemWebHookToken(string webHookToken)
    {
        var httpClientFactory = new Mock<IHttpClientFactory>();
        var discordHttpService = new DiscordHttpService(httpClientFactory.Object);

        var discordModel = new DiscordModel()
        {
            Content = "Error expeptions",
            Username = "Error",
            Embeds =
            [
                new()
                {
                    Description = "Error",
                    Title = "Error api",
                    Color = 0xFF0000
                }
            ]
        };

        await Assert.ThrowsAnyAsync<Exception>(
            async () => await discordHttpService.NotifyExceptionAsync(discordModel, "123", webHookToken));
    }
}
