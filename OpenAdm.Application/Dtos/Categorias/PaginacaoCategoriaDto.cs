using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Application.PaginateDto;

public class PaginacaoCategoriaDto : FilterModel<Categoria>
{
    public override Expression<Func<Categoria, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => x.Numero.ToString().Contains(Search.ToLower())
            || x.Descricao.Contains(Search.ToLower());
    }
}
