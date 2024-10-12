using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.ContasAReceberModel;

public class ContasAReceberViewModel : BaseViewModel
{
    public StatusContasAPagarEnum Status { get; set; }
    public Guid UsuarioId { get; set; }
    public UsuarioViewModel Usuario { get; set; } = null!;
    public Guid? PedidoId { get; set; }
    public PedidoViewModel? Pedido { get; set; }
    public DateTime? DataDeFechamento { get;  set; }
    public IList<FaturaContasAReceberViewModel> Faturas { get; set; } = [];
    public decimal Total { get { return Faturas.Sum(x => x.Valor); } }

    public static explicit operator ContasAReceberViewModel(ContasAReceber contasAReceber)
    {
        return new ContasAReceberViewModel()
        {
            DataDeCriacao = contasAReceber.DataDeCriacao,
            DataDeFechamento = contasAReceber.DataDeFechamento,
            Faturas = contasAReceber.Faturas.Select(x => (FaturaContasAReceberViewModel)x).ToList(),
            Id = contasAReceber.Id,
            Numero = contasAReceber.Numero,
            Pedido = contasAReceber.Pedido == null ? null : new PedidoViewModel().ForModel(contasAReceber.Pedido),
            PedidoId = contasAReceber.PedidoId,
            Status = contasAReceber.Status,
            UsuarioId = contasAReceber.UsuarioId,
            Usuario = contasAReceber.Usuario == null ? null! : new UsuarioViewModel().ToModel(contasAReceber.Usuario)
        };
    }
}
