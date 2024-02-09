using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace OpenAdm.Infra.Model;

public class DiscordModel
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
    [JsonPropertyName("embeds")]
    public DiscordEmbeds[]? Embeds { get; set; }

    public StringContent ToJson()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        });

        return new StringContent(
                json,
                Encoding.UTF8,
                "application/json");
    }
}
