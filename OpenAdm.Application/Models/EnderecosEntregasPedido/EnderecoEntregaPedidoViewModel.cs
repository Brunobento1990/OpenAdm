using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.EnderecosEntregasPedido;

public class EnderecoEntregaPedidoViewModel
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public decimal ValorFrete { get; set; }
    public string TipoFrete { get; set; } = string.Empty;
    public Guid PedidoId { get; set; }

    public static explicit operator EnderecoEntregaPedidoViewModel(EnderecoEntregaPedido enderecoEntregaPedido)
    {
        return new EnderecoEntregaPedidoViewModel()
        {
            Bairro = enderecoEntregaPedido.Bairro,
            Cep = enderecoEntregaPedido.Cep,
            Complemento = enderecoEntregaPedido.Complemento,
            Id = enderecoEntregaPedido.Id,
            Localidade = enderecoEntregaPedido.Localidade,
            Logradouro = enderecoEntregaPedido.Logradouro,
            Numero = enderecoEntregaPedido.Numero,
            PedidoId = enderecoEntregaPedido.PedidoId,
            TipoFrete = enderecoEntregaPedido.TipoFrete,
            Uf = enderecoEntregaPedido.Uf,
            ValorFrete = enderecoEntregaPedido.ValorFrete
        };
    }
}
