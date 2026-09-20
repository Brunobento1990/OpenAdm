using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class FuncionarioBuilder
{
    private Guid _id = Guid.NewGuid();
    private readonly DateTime _dataDeCriacao = DateTime.UtcNow;
    private readonly DateTime _dataDeAtualizacao = DateTime.UtcNow;
    private readonly long _numero = new Faker().Random.Long(1, 10000);
    private string _email = "funcionario@email.com";
    private string _senha = "senha";
    private string _nome = "Funcionário";
    private Guid _parceiroId = Guid.NewGuid();

    public static FuncionarioBuilder Init() => new();

    public FuncionarioBuilder ComId(Guid id) { _id = id; return this; }
    public FuncionarioBuilder ComSenha(string senha) { _senha = senha; return this; }
    public FuncionarioBuilder ComParceiroId(Guid parceiroId) { _parceiroId = parceiroId; return this; }

    public Funcionario Build() => new(
        _id, _dataDeCriacao, _dataDeAtualizacao, _numero, _email, _senha, _nome,
        null, null, true, _parceiroId);
}
