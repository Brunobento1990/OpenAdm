using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Pedidos;

public class ProcessarPedidoModel
{
    public string EmailEnvio { get; set; } = string.Empty;
    public string NomeEmpresa { get; set; } = string.Empty;
    public byte[]? Logo { get; set; }
    public Pedido Pedido { get; set; } = null!;
}
