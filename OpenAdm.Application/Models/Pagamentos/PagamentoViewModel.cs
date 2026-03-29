using System.Text.Json.Serialization;

namespace OpenAdm.Application.Models.Pagamentos;

public class PagamentoViewModel
{
    public string? QrCodePix { get; set; }
    public string? QrCodePixBase64 { get; set; }
    public string? LinkPagamento { get; set; }
    [JsonIgnore]
    public string IdExterno { get; set; } = string.Empty;
}
