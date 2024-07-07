using System.Text.Json.Serialization;

namespace OpenAdm.Application.Dtos.Response;

public class ErrorResponse
{
    [JsonPropertyName("mensagem")]
    public string Mensagem { get; set; } = string.Empty;
}
