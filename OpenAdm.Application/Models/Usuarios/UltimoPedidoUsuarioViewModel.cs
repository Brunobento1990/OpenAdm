using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.Usuarios;

public class UltimoPedidoUsuarioViewModel
{
    public Guid UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CpfCnpj { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public Guid? PedidoId { get; set; }
    public DateTime? DataDoUltimoPedido { get; set; }
    public decimal Total { get; set; }
    public long NumeroDoPedido { get; set; }
    public StatusPedido? StatusPedido { get; set; }
}

public class PaginacaoUltimoPedidoUsuarioViewModel
{
    public IEnumerable<UltimoPedidoUsuarioViewModel> Dados { get; set; } = [];
    public int TotalPagina { get; set; }
}
