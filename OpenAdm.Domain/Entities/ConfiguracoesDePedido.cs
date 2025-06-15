using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class ConfiguracoesDePedido : BaseEntity
{
    public ConfiguracoesDePedido(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string emailDeEnvio,
        bool ativo,
        decimal? pedidoMinimoAtacado,
        decimal? pedidoMinimoVarejo)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        EmailDeEnvio = emailDeEnvio;
        Ativo = ativo;
        PedidoMinimoAtacado = pedidoMinimoAtacado;
        PedidoMinimoVarejo = pedidoMinimoVarejo;
    }

    public void Update(string emailDeEnvio, bool ativo, decimal? pedidoMinimoAtacado, decimal? pedidoMinimoVarejo)
    {
        PedidoMinimoAtacado = pedidoMinimoAtacado;
        PedidoMinimoVarejo = pedidoMinimoVarejo;
        EmailDeEnvio = emailDeEnvio;
        Ativo = ativo;
    }

    public string EmailDeEnvio { get; private set; }
    public bool Ativo { get; private set; }
    public decimal? PedidoMinimoAtacado { get; private set; }
    public decimal? PedidoMinimoVarejo { get; private set; }
}
