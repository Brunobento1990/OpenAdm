using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Domain.PaginateDto;

public class PaginacaoCategoriaDto : FilterModel
{
    public Expression<Func<Categoria, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => x.Numero.ToString().Contains(Search.ToLower())
            || x.Descricao.Contains(Search.ToLower());
    }
}
