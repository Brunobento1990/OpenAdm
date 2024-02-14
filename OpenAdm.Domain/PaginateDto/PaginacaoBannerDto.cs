using OpenAdm.Domain.Entities;
using System.Linq.Expressions;

namespace OpenAdm.Domain.Model.PaginateDto;

public class PaginacaoBannerDto : FilterModel
{
    public Expression<Func<Banner, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => x.Numero.ToString().ToLower().Contains(Search.ToLower());
    }
}
