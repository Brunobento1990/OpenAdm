using System.Text.Json.Serialization;

namespace OpenAdm.Infra.Model;

public class DiscordEmbeds
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("color")]
    public int? Color { get; set; }
}
