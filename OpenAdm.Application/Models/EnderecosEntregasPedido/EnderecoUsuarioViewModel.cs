using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.EnderecosEntregasPedido;

public class EnderecoUsuarioViewModel
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public Guid Id { get; set; }

    public static explicit operator EnderecoUsuarioViewModel(EnderecoUsuario enderecoUsuario)
    {
        return new EnderecoUsuarioViewModel()
        {
            Bairro = enderecoUsuario.Bairro,
            Cep = enderecoUsuario.Cep,
            Complemento = enderecoUsuario.Complemento,
            Id = enderecoUsuario.Id,
            Localidade = enderecoUsuario.Localidade,
            Logradouro = enderecoUsuario.Logradouro,
            Numero = enderecoUsuario.Numero,
            Uf = enderecoUsuario.Uf
        };
    }
}
