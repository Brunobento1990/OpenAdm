using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAdm.Domain.Helpers;

public static class JsonSerializerOptionsApi
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public static JsonSerializerOptions Options =>  _options;

    public static T? FromJson<T>(this Stream json)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }
        catch (Exception e)
        {
            return default;
        }
    }

    public static StringContent ToJson<T>(T body)
    {
        var json = JsonSerializer.Serialize(body, new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}