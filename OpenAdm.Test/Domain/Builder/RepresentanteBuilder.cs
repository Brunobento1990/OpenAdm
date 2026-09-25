using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Domain.Builder;

public sealed class RepresentanteBuilder
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly DateTime _data = DateTime.UtcNow;
    private string _nome = "Representante Teste";
    private string? _cpf = "52998224725";
    private string? _email = "representante@teste.com";
    private string? _telefone = "11999999999";
    private string? _senha = "hash";
    private bool _ativo = true;

    public static RepresentanteBuilder Init() => new();

    public RepresentanteBuilder ComNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public RepresentanteBuilder Inativo()
    {
        _ativo = false;
        return this;
    }

    public Representante Build() =>
        new(_id, _data, _data, 1, _nome, _cpf, _email, _telefone, _senha, _ativo);
}
