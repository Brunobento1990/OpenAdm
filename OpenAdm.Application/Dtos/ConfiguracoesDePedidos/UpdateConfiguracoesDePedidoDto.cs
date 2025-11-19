using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.ConfiguracoesDePedidos;

public class UpdateConfiguracoesDePedidoDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [MaxLength(255)]
    public string EmailDeEnvio { get; set; } = string.Empty;
    public string? WhatsApp { get; set; }
    public decimal? PedidoMinimoAtacado { get; set; }
    public decimal? PedidoMinimoVarejo { get; set; }
    public bool VendaDeProdutoComEstoque { get; set; } = false;
}
