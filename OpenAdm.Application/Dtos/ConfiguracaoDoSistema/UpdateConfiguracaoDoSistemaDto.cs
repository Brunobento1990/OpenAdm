using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.ConfiguracaoDoSistema;

public class UpdateConfiguracaoDoSistemaDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [MaxLength(255)]
    public string EmailParaEnvioNotificacao { get; set; } = string.Empty;
    public string? WhatsAppParaEnvioNotificacao { get; set; }
    public decimal? PedidoMinimoAtacado { get; set; }
    public decimal? PedidoMinimoVarejo { get; set; }
    public bool VendaDeProdutoComEstoque { get; set; } = false;
    public bool ListarProdutoSomenteComEstoque { get; set; } = false;
    public bool NotificarCadastroNovoCliente { get; set; } = false;
    [MaxLength(3000)]
    public string? MensagemPedidoFinalizado { get; set; }
}
