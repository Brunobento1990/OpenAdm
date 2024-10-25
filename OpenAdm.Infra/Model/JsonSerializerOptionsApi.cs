using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace OpenAdm.Infra.Model;

public static class JsonSerializerOptionsApi
{
    private static readonly JsonSerializerOptions _options = new()
    {

        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };
    public static JsonSerializerOptions Options()
    {
        return _options;
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
