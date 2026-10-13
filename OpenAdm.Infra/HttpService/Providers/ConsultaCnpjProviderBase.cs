using System.Text.Json;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Providers;

public abstract class ConsultaCnpjProviderBase
{
    protected static async Task<T> ObterRespostaAsync<T>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ExceptionApi("Não foi possível obter a resposta da consulta do seu CNPJ");
            }

            var erro = JsonSerializer.Deserialize<ConsultaCnpjErroResponse>(body, JsonSerializerOptionsApi.Options);

            if (!string.IsNullOrWhiteSpace(erro?.Message))
            {
                throw new ExceptionApi(erro.Message);
            }

            throw new ExceptionApi("Não foi possível efetuar a consulta do seu CNPJ");
        }

        return JsonSerializer.Deserialize<T>(body, JsonSerializerOptionsApi.Options)
               ?? throw new ExceptionApi("Não foi possível efetuar a consulta do seu CNPJ!");
    }
}
