using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Application.Model.PaginateDto;

public class PaginacaoBannerDto : FilterModel<Banner>
{
    public override Expression<Func<Banner, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => x.Numero.ToString().ToLower().Contains(Search.ToLower());
    }
}
