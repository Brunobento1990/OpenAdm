using ConfiguracaoDoSistemaEntity = OpenAdm.Domain.Entities.ConfiguracaoDoSistema;

namespace OpenAdm.Application.Models.ConfiguracaoDoSistema;

public class ConfiguracaoDoSistemaViewModel : BaseModel
{
    public string EmailParaEnvioNotificacao { get; set; } = string.Empty;
    public string? WhatsAppParaEnvioNotificacao { get; set; }
    public decimal? PedidoMinimoAtacado { get; set; }
    public decimal? PedidoMinimoVarejo { get; set; }
    public bool VendaDeProdutoComEstoque { get; set; } = false;
    public bool ListarProdutoSomenteComEstoque { get; set; } = false;
    public bool NotificarCadastroNovoCliente { get; set; } = false;
    public string? MensagemPedidoFinalizado { get; set; }


    public ConfiguracaoDoSistemaViewModel ToModel(ConfiguracaoDoSistemaEntity configuracaoDoSistema)
    {
        Id = configuracaoDoSistema.Id;
        DataDeCriacao = configuracaoDoSistema.DataDeCriacao;
        DataDeAtualizacao = configuracaoDoSistema.DataDeAtualizacao;
        Numero = configuracaoDoSistema.Numero;
        EmailParaEnvioNotificacao = configuracaoDoSistema.EmailParaEnvioNotificacao;
        PedidoMinimoAtacado = configuracaoDoSistema.PedidoMinimoAtacado;
        PedidoMinimoVarejo = configuracaoDoSistema.PedidoMinimoVarejo;
        WhatsAppParaEnvioNotificacao = configuracaoDoSistema.WhatsAppParaEnvioNotificacao;
        VendaDeProdutoComEstoque = configuracaoDoSistema.VendaDeProdutoComEstoque;
        ListarProdutoSomenteComEstoque = configuracaoDoSistema.ListarProdutoSomenteComEstoque;
        MensagemPedidoFinalizado = configuracaoDoSistema.MensagemPedidoFinalizado;
        NotificarCadastroNovoCliente = configuracaoDoSistema.NotificarCadastroNovoCliente;

        return this;
    }
}
