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
        byte[]? logo,
        bool ativo,
        decimal? pedidoMinimoAtacado,
        decimal? pedidoMinimoVarejo)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        EmailDeEnvio = emailDeEnvio;
        Ativo = ativo;
        Logo = logo;
        PedidoMinimoAtacado = pedidoMinimoAtacado;
        PedidoMinimoVarejo = pedidoMinimoVarejo;
    }

    public void Update(string emailDeEnvio, bool ativo, byte[]? logo, decimal? pedidoMinimoAtacado, decimal? pedidoMinimoVarejo)
    {
        PedidoMinimoAtacado = pedidoMinimoAtacado;
        PedidoMinimoVarejo = pedidoMinimoVarejo;
        EmailDeEnvio = emailDeEnvio;
        Ativo = ativo;
        Logo = logo;
    }

    public string EmailDeEnvio { get; private set; }
    public bool Ativo { get; private set; }
    public byte[]? Logo { get; private set; }
    public decimal? PedidoMinimoAtacado { get; private set; }
    public decimal? PedidoMinimoVarejo { get; private set; }
}
