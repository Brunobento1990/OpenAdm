﻿using OpenAdm.Infra.HttpService.Interfaces;
using System.Net.Http.Headers;
using OpenAdm.Infra.Model;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Infra.HttpService.Services;

public class DiscordHttpService(IHttpClientFactory httpClientFactory) 
    : IDiscordHttpService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task NotifyExceptionAsync(string message, string webHookId, string webHookToken)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ExceptionApi("Mensagem para notificação inválida!");

        if (string.IsNullOrWhiteSpace(webHookId))
            throw new ExceptionApi("Web hook ID do discord inválido!");

        if (string.IsNullOrWhiteSpace(webHookToken))
            throw new ExceptionApi("Web hook token do discord inválido!");

        var url = $"{webHookId}/{webHookToken}";
        var httpClient = _httpClientFactory.CreateClient("Discord");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        await httpClient.PostAsync(url, BodyErrorDiscord(message));
    }

    private static StringContent BodyErrorDiscord(string message)
    {
        var discordModel = new DiscordModel()
        {
            Content = "Error expeptions",
            Username = "Error",
            Embeds =
            [
                new()
                {
                    Description = message,
                    Title = "Error api",
                    Color = 0xFF0000
                }
            ]
        };

        return discordModel.ToJson();
    }
}
