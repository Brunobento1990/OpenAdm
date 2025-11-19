using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.ConfiguracoesDePedidos;

public class ConfiguracoesDePedidoViewModel : BaseModel
{
    public string EmailDeEnvio { get; set; } = string.Empty;
    public string? WhatsApp { get; set; }
    public decimal? PedidoMinimoAtacado { get; set; }
    public decimal? PedidoMinimoVarejo { get; set; }
    public bool VendaDeProdutoComEstoque { get; set; } = false;


    public ConfiguracoesDePedidoViewModel ToModel(ConfiguracoesDePedido configuracoesDePedido)
    {
        Id = configuracoesDePedido.Id;
        DataDeCriacao = configuracoesDePedido.DataDeCriacao;
        DataDeAtualizacao = configuracoesDePedido.DataDeAtualizacao;
        Numero = configuracoesDePedido.Numero;
        EmailDeEnvio = configuracoesDePedido.EmailDeEnvio;
        PedidoMinimoAtacado = configuracoesDePedido.PedidoMinimoAtacado;
        PedidoMinimoVarejo = configuracoesDePedido.PedidoMinimoVarejo;
        WhatsApp = configuracoesDePedido.WhatsApp;
        VendaDeProdutoComEstoque = configuracoesDePedido.VendaDeProdutoComEstoque;

        return this;
    }
}
