using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class ConfiguracoesDePedido : BaseEntityParceiro
{
    public ConfiguracoesDePedido(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string emailDeEnvio,
        bool ativo,
        decimal? pedidoMinimoAtacado,
        decimal? pedidoMinimoVarejo,
        Guid parceiroId,
        string? whatsApp,
        bool vendaDeProdutoComEstoque)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero, parceiroId)
    {
        EmailDeEnvio = emailDeEnvio;
        Ativo = ativo;
        PedidoMinimoAtacado = pedidoMinimoAtacado;
        PedidoMinimoVarejo = pedidoMinimoVarejo;
        WhatsApp = whatsApp;
        VendaDeProdutoComEstoque = vendaDeProdutoComEstoque;
    }

    public void Update(string emailDeEnvio, bool ativo, decimal? pedidoMinimoAtacado, decimal? pedidoMinimoVarejo, string? whatsApp, bool vendaDeProdutoComEstoque)
    {
        PedidoMinimoAtacado = pedidoMinimoAtacado;
        PedidoMinimoVarejo = pedidoMinimoVarejo;
        EmailDeEnvio = emailDeEnvio;
        Ativo = ativo;
        WhatsApp = whatsApp;
        VendaDeProdutoComEstoque = vendaDeProdutoComEstoque;
    }

    public string EmailDeEnvio { get; private set; }
    public string? WhatsApp { get; private set; }
    public bool Ativo { get; private set; }
    public bool VendaDeProdutoComEstoque { get; private set; } = false;
    public decimal? PedidoMinimoAtacado { get; private set; }
    public decimal? PedidoMinimoVarejo { get; private set; }
}
