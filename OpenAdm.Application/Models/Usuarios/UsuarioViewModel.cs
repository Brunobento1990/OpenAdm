using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Usuarios;

public class UsuarioViewModel : BaseModel
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public int? QuantidadeDePedido { get; set; }
    public int? PedidosEmAberto { get; set; }
    public int? PedidosFaturado { get; set; }
    public int? PedidosEmEntraga { get; set; }
    public int? PedidosEntregue { get; set; }
    public int? PedidosCancelados { get; set; }
    public decimal? TotalPedido { get; set; }
    public bool IsAtacado { get; set; }

    public UsuarioViewModel ToModel(Usuario entity, int? quantidadeDePedido = null)
    {
        QuantidadeDePedido = quantidadeDePedido;
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Email = entity.Email;
        Numero = entity.Numero;
        Telefone = entity.Telefone;
        Nome = entity.Nome;
        Cnpj = entity.Cnpj;
        Cpf = entity.Cpf;
        IsAtacado = entity.IsAtacado;

        return this;
    }
}
