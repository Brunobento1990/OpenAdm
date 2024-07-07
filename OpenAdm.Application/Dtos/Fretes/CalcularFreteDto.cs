using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace OpenAdm.Application.Dtos.Fretes;

public class CalcularFreteDto
{
    public string CepOrigem { get; set; } = string.Empty;
    public string CepDestino { get; set; } = string.Empty;
    public string Altura { get; set; } = string.Empty;
    public string Largura { get; set; } = string.Empty;
    public string Comprimento { get; set; } = string.Empty;
    public string TipoFrete { get; set; } = string.Empty;
    public int Peso { get; set; }

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
