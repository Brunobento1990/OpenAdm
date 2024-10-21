using System.Text.Json.Serialization;
using System.Text.Json;

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
}
