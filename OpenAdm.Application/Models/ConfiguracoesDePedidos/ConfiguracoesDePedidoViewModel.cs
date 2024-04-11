using Domain.Pkg.Entities;
using System.Text;

namespace OpenAdm.Application.Models.ConfiguracoesDePedidos;

public class ConfiguracoesDePedidoViewModel : BaseModel
{
    public string EmailDeEnvio { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public decimal? PedidoMinimoAtacado { get; set; }
    public decimal? PedidoMinimoVarejo { get; set; }

    public ConfiguracoesDePedidoViewModel ToModel(ConfiguracoesDePedido configuracoesDePedido)
    {
        Id = configuracoesDePedido.Id;
        DataDeCriacao = configuracoesDePedido.DataDeCriacao;
        DataDeAtualizacao = configuracoesDePedido.DataDeAtualizacao;
        Numero = configuracoesDePedido.Numero;
        EmailDeEnvio = configuracoesDePedido.EmailDeEnvio;
        Logo = configuracoesDePedido.Logo == null ? null : Encoding.UTF8.GetString(configuracoesDePedido.Logo);
        PedidoMinimoAtacado = configuracoesDePedido.PedidoMinimoAtacado;
        PedidoMinimoVarejo = configuracoesDePedido.PedidoMinimoVarejo;

        return this;
    }
}
