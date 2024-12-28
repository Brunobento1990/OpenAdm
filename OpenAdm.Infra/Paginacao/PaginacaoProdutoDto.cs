using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoProdutoDto : FilterModel<Produto>
{
    public override Expression<Func<Produto, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        var pesquisa = Search.ToLower();
        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao.ToLower()), $"%{pesquisa}%")
            || EF.Functions.ILike(EF.Functions.Unaccent(x.Categoria.Descricao.ToLower()), $"%{pesquisa}%");
    }

    public override Expression<Func<Produto, object>>? IncludeCustom()
    {
        return x => x.Categoria;
    }
}
