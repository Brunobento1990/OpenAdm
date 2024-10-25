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
        return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
    }
}
