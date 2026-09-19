using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Application.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAdm.Infra.Cached.Services;

public class CachedService<T> : ICachedService<T> where T : class
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _options;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly IParceiroAutenticado _parceiroAutenticado;

    public CachedService(
        IDistributedCache distributedCache,
        IParceiroAutenticado parceiroAutenticado,
        IConfiguration configuration)
    {
        var expiracaoAbsolutaMinutos = ObterMinutosConfigurados(
            configuration,
            "Cache:ExpiracaoAbsolutaMinutos",
            15);
        var expiracaoDeslizanteMinutos = ObterMinutosConfigurados(
            configuration,
            "Cache:ExpiracaoDeslizanteMinutos",
            7);

        _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
        _options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(expiracaoAbsolutaMinutos))
            .SetSlidingExpiration(TimeSpan.FromMinutes(expiracaoDeslizanteMinutos));

        _distributedCache = distributedCache;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<T?> GetItemAsync(string key)
    {
        Valid(key);
        var value = await _distributedCache.GetStringAsync(NewKey(key));
        return value is null ? null : JsonSerializer.Deserialize<T>(value, _serializerOptions);
    }

    public async Task<IList<T>?> GetListItemAsync(string key)
    {
        Valid(key);
        var values = await _distributedCache.GetStringAsync(NewKey(key));
        return values is null ? null : JsonSerializer.Deserialize<List<T>>(values, _serializerOptions);
    }

    public async Task RemoveCachedAsync(string key)
    {
        Valid(key);
        await _distributedCache.RemoveAsync(NewKey(key));
    }

    public async Task SetItemAsync(
        string key,
        T item,
        TimeSpan? tempoExpiracao = null,
        TimeSpan? tempoExpiracaoDeslizante = null)
    {
        Valid(key);
        var valueJson = JsonSerializer.Serialize(item, options: _serializerOptions);
        var options = tempoExpiracao.HasValue || tempoExpiracaoDeslizante.HasValue
            ? CopiarOptionsComExpiracaoPersonalizada(tempoExpiracao, tempoExpiracaoDeslizante)
            : _options;

        await _distributedCache.SetStringAsync(NewKey(key), valueJson, options);
    }

    public async Task SetListItemAsync(string key, IList<T> itens)
    {
        Valid(key);
        var valuesJson = JsonSerializer.Serialize(itens, options: _serializerOptions);
        await _distributedCache.SetStringAsync(NewKey(key), valuesJson, _options);
    }

    private static void Valid(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new Exception("Key do cached inválida!");
    }

    private string NewKey(string key)
    {
        return $"{_parceiroAutenticado.Id}_{key}";
    }

    private DistributedCacheEntryOptions CopiarOptionsComExpiracaoPersonalizada(
        TimeSpan? tempoExpiracao,
        TimeSpan? tempoExpiracaoDeslizante)
    {
        ValidarTempoExpiracao(tempoExpiracao, nameof(tempoExpiracao));
        ValidarTempoExpiracao(tempoExpiracaoDeslizante, nameof(tempoExpiracaoDeslizante));

        return new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = tempoExpiracao ?? _options.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = tempoExpiracaoDeslizante ?? _options.SlidingExpiration
        };
    }

    private static void ValidarTempoExpiracao(TimeSpan? tempoExpiracao, string nomeParametro)
    {
        if (tempoExpiracao.HasValue && tempoExpiracao.Value <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nomeParametro,
                "O tempo de expiração deve ser maior que zero.");
        }
    }

    private static double ObterMinutosConfigurados(
        IConfiguration configuration,
        string chave,
        double valorPadrao)
    {
        return double.TryParse(configuration[chave], out var minutos) && minutos > 0
            ? minutos
            : valorPadrao;
    }
}
