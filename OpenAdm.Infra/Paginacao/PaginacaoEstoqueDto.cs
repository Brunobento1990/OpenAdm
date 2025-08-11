using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoEstoqueDto : FilterModel<Estoque>
{
    public override IList<Expression<Func<Estoque, object>>>? IncludeCustomList()
    {
        List<Expression<Func<Estoque, object>>> includes = [];
        includes.Add(x => x.Produto);
        includes.Add(x => x.Peso!);
        includes.Add(x => x.Tamanho!);

        return includes;
    }

    public override Expression<Func<Estoque, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        var search = Search.RemoverAcentos();

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Numero.ToString()), $"%{search}%") ||
            EF.Functions.ILike(EF.Functions.Unaccent(x.Produto.Descricao), $"%{search}%");
    }
}
