using System.Linq.Expressions;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Model;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoParcelaCobrancaDTO : FilterModel<ParcelaCobranca>
{
    public override Expression<Func<ParcelaCobranca, bool>>? GetWhereBySearch()
    {
        throw new NotImplementedException();
    }
}