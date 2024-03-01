using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.ConfiguracoesDePedidos;

public class ConfiguracoesDePedidoViewModel : BaseModel
{
    public string EmailDeEnvio { get; set; } = string.Empty;
    public string? Logo { get; set; }

    public ConfiguracoesDePedidoViewModel ToModel(ConfiguracoesDePedido configuracoesDePedido)
    {
        Id = configuracoesDePedido.Id;
        DataDeCriacao = configuracoesDePedido.DataDeCriacao;
        DataDeAtualizacao = configuracoesDePedido.DataDeAtualizacao;
        Numero = configuracoesDePedido.Numero;
        EmailDeEnvio = configuracoesDePedido.EmailDeEnvio;

        return this;
    }
}
