using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Application.Models.FaturasModel;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.ContasAReceberModel;

public class FaturaViewModel : BaseViewModel
{
    public StatusFaturaEnum Status { get; set; }
    public TipoFaturaEnum Tipo { get; set; }
    public Guid UsuarioId { get; set; }
    public UsuarioViewModel Usuario { get; set; } = null!;
    public Guid? PedidoId { get; set; }
    public PedidoViewModel? Pedido { get; set; }
    public DateTime? DataDeFechamento { get;  set; }
    public IList<ParcelaViewModel> Parcelas { get; set; } = [];
    public decimal Total { get { return Parcelas.Sum(x => x.Valor); } }

    public static explicit operator FaturaViewModel(Fatura fatura)
    {
        return new FaturaViewModel()
        {
            DataDeCriacao = fatura.DataDeCriacao,
            DataDeFechamento = fatura.DataDeFechamento,
            Parcelas = fatura.Parcelas.Select(x => (ParcelaViewModel)x).ToList(),
            Id = fatura.Id,
            Numero = fatura.Numero,
            Pedido = fatura.Pedido == null ? null : new PedidoViewModel().ForModel(fatura.Pedido),
            PedidoId = fatura.PedidoId,
            Status = fatura.Status,
            UsuarioId = fatura.UsuarioId,
            Usuario = fatura.Usuario == null ? null! : new UsuarioViewModel().ToModel(fatura.Usuario),
            Tipo = fatura.Tipo
        };
    }
}
