using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoBannerDto : FilterModel<Banner>
{
    public override Expression<Func<Banner, bool>> GetWhereBySearch()
    {
        if (ListarInativo)
        {
            return x => x.ParceiroId == ParceiroId;
        }

        return x => x.ParceiroId == ParceiroId && x.Ativo;
    }
}