﻿using Microsoft.Extensions.Caching.Distributed;
using Domain.Pkg.Exceptions;
using OpenAdm.Infra.Cached.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace OpenAdm.Infra.Cached.Services;

public class CachedService<T> : ICachedService<T> where T : class
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _options;
    private readonly JsonSerializerOptions _serializerOptions;
    private static readonly double _absolutExpiration = 60;
    private static readonly double _slidingExpiration = 30;
    private readonly string _dominimo;

    public CachedService(IDistributedCache distributedCache,
        IHttpContextAccessor httpContextAccessor)
    {
        _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };
        _options = new DistributedCacheEntryOptions()
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(_absolutExpiration))
                      .SetSlidingExpiration(TimeSpan.FromMinutes(_slidingExpiration));

        _distributedCache = distributedCache;
        _dominimo = httpContextAccessor?
           .HttpContext?
           .Request?
           .Headers?
           .FirstOrDefault(x => x.Key == "Referer").Value.ToString() ?? throw new Exception();
    }

    public async Task<T?> GetItemAsync(string key)
    {
        Valid(key);
        var value = await _distributedCache.GetStringAsync(GetNewKey(key));
        return value is null ? null : JsonSerializer.Deserialize<T>(value, _serializerOptions);
    }

    public async Task<IList<T>?> GetListItemAsync(string key)
    {
        Valid(key);
        var values = await _distributedCache.GetStringAsync(GetNewKey(key));
        return values is null ? null : JsonSerializer.Deserialize<List<T>>(values, _serializerOptions);
    }

    public async Task RemoveCachedAsync(string key)
    {
        Valid(key);
        await _distributedCache.RemoveAsync(GetNewKey(key));
    }

    public async Task SetItemAsync(string key, T item)
    {
        Valid(key);
        var valueJson = JsonSerializer.Serialize<T>(item, options: _serializerOptions);
        await _distributedCache.SetStringAsync(GetNewKey(key), valueJson, _options);
    }

    public async Task SetListItemAsync(string key, IList<T> itens)
    {
        Valid(key);
        var valuesJson = JsonSerializer.Serialize<IList<T>>(itens, options: _serializerOptions);
        await _distributedCache.SetStringAsync(GetNewKey(key), valuesJson, _options);
    }

    private static void Valid(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ExceptionApi("Key do cached inválida!");
    }

    private string GetNewKey(string key)
    {
        return $"{_dominimo}-{key}";
    }
}
