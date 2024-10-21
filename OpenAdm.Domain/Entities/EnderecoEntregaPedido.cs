using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class EnderecoEntregaPedido : BaseEndereco
{
    public EnderecoEntregaPedido(
        string cep,
        string logradouro,
        string bairro,
        string localidade,
        string complemento,
        string numero,
        string uf,
        Guid pedidoId,
        decimal valorFrete,
        string tipoFrete,
        Guid id)
            : base(cep, logradouro, bairro, localidade, complemento, numero, uf)
    {
        PedidoId = pedidoId;
        ValorFrete = valorFrete;
        TipoFrete = tipoFrete;
        Id = id;
    }
    public Guid Id { get; private set; }
    public decimal ValorFrete { get; private set; }
    public string TipoFrete { get; private set; }
    public Guid PedidoId { get; private set; }
    public Pedido Pedido { get; set; } = null!;
}
