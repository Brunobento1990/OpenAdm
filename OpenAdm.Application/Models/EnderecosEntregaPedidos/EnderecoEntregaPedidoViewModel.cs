using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.EnderecosEntregaPedidos;

public class EnderecoEntregaPedidoViewModel : BaseModel
{
    public string Cep { get; set; } = string.Empty;
    public decimal Frete { get; set; }
    public string Logradouro { get; set; } = string.Empty;
    public string NumeroEntrega { get; set; } = string.Empty;
    public Guid PedidoId { get; set; }
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string TipoFrete { get; set; } = string.Empty;

    public static EnderecoEntregaPedidoViewModel? ToEntity(EnderecoEntregaPedido? enderecoEntregaPedido)
    {
        if (enderecoEntregaPedido == null) return null;

        return new EnderecoEntregaPedidoViewModel()
        {
            Bairro = enderecoEntregaPedido.Bairro,
            Cep = enderecoEntregaPedido.Cep,
            Complemento = enderecoEntregaPedido.Complemento,
            DataDeAtualizacao = enderecoEntregaPedido.DataDeAtualizacao,
            DataDeCriacao = enderecoEntregaPedido.DataDeCriacao,
            Frete = enderecoEntregaPedido.Frete,
            Id = enderecoEntregaPedido.Id,
            Localidade = enderecoEntregaPedido.Localidade,
            Logradouro = enderecoEntregaPedido.Logradouro,
            Numero = enderecoEntregaPedido.Numero,
            NumeroEntrega = enderecoEntregaPedido.NumeroEntrega,
            PedidoId = enderecoEntregaPedido.PedidoId,
            TipoFrete = enderecoEntregaPedido.TipoFrete,
            Uf = enderecoEntregaPedido.Uf
        };
    }
}
