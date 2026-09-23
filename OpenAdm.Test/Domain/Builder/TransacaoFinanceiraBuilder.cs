using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Test.Domain.Builder;

public class TransacaoFinanceiraBuilder
{
    private DateTime _dataDeEfetivacao = DateTime.UtcNow;

    public static TransacaoFinanceiraBuilder Init() => new();

    public TransacaoFinanceiraBuilder ComDataDeEfetivacao(DateTime data)
    {
        _dataDeEfetivacao = data;
        return this;
    }

    public TransacaoFinanceira Build() => new(
        Guid.NewGuid(), _dataDeEfetivacao, _dataDeEfetivacao, 0, null,
        _dataDeEfetivacao, 100, TipoTransacaoFinanceiraEnum.Entrada,
        null, null, false, null, null);
}
