using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class EnderecoParceiro : BaseEndereco
{
    public EnderecoParceiro(
        string cep,
        string logradouro,
        string bairro,
        string localidade,
        string complemento,
        string numero,
        string uf,
        Guid id,
        Guid parceiroId)
            : base(cep, logradouro, bairro, localidade, complemento, numero, uf)
    {
        Id = id;
        ParceiroId = parceiroId;
    }

    public void Editar(
        string cep,
        string logradouro,
        string bairro,
        string localidade,
        string complemento,
        string numero,
        string uf)
    {
        Cep = cep;
        Logradouro = logradouro;
        Bairro = bairro;
        Localidade = localidade;
        Complemento = complemento;
        Numero = numero;
        Uf = uf;
    }

    public Guid Id { get; private set; }
    public Guid ParceiroId { get; private set; }
    public Parceiro Parceiro { get; set; } = null!;
}
