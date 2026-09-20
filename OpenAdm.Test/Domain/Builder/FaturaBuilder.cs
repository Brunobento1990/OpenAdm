using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Test.Domain.Builder;

public class FaturaBuilder
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly DateTime _dataDeCriacao = DateTime.UtcNow;
    private readonly DateTime _dataDeAtualizacao = DateTime.UtcNow;
    private readonly long _numero = new Faker().Random.Long(1, 10000);
    private Guid _usuarioId = Guid.NewGuid();
    private Guid? _pedidoId;
    private StatusFaturaEnum _status = StatusFaturaEnum.Aberta;
    private TipoFaturaEnum _tipo = TipoFaturaEnum.AReceber;
    private decimal _total = 100;

    public static FaturaBuilder Init() => new();

    public FaturaBuilder ComPedido(Pedido pedido)
    {
        _pedidoId = pedido.Id;
        _usuarioId = pedido.UsuarioId;
        return this;
    }

    public Fatura Build() => new(
        _id,
        _dataDeCriacao,
        _dataDeAtualizacao,
        _numero,
        _status,
        _usuarioId,
        _pedidoId,
        null,
        _tipo,
        _total);
}
