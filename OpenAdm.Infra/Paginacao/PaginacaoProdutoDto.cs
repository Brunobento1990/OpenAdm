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
            return ListarInativo ? null : x => x.Ativo;

        var pesquisa = Search.ToLower();
        if (ListarInativo)
            return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao.ToLower()), $"%{pesquisa}%")
                        || EF.Functions.ILike(EF.Functions.Unaccent(x.Categoria.Descricao.ToLower()), $"%{pesquisa}%")
                        || EF.Functions.ILike(EF.Functions.Unaccent(x.Referencia!), $"%{pesquisa}%");

        return x => x.Ativo &&
                    (EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao.ToLower()), $"%{pesquisa}%")
                     || EF.Functions.ILike(EF.Functions.Unaccent(x.Categoria.Descricao.ToLower()), $"%{pesquisa}%")
                     || EF.Functions.ILike(EF.Functions.Unaccent(x.Referencia!), $"%{pesquisa}%"));
    }

    public override Expression<Func<Produto, object>>? IncludeCustom()
    {
        return x => x.Categoria;
    }
}
