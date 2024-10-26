namespace OpenAdm.Domain.Entities.Bases;

public abstract class BaseEndereco
{
    protected BaseEndereco(
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

    public string Cep { get; protected set; }
    public string Logradouro { get; protected set; }
    public string Bairro { get; protected set; }
    public string Localidade { get; protected set; }
    public string Complemento { get; protected set; }
    public string Numero { get; protected set; }
    public string Uf { get; protected set; }
}
