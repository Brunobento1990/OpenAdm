using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;
using OpenAdm.Domain.Extensions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoMovimentacaoDeProdutoDto : FilterModel<MovimentacaoDeProduto>
{
    public override IList<Expression<Func<MovimentacaoDeProduto, object>>>? IncludeCustomList()
    {
        return new List<Expression<Func<MovimentacaoDeProduto, object>>>()
        {
            { x => x.Produto },
            { x => x.Peso! },
            { x => x.Tamanho! },
        };
    }

    public override Expression<Func<MovimentacaoDeProduto, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        var search = Search.RemoverAcentos().Trim();
        
        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Produto.Descricao), $"%{search}%");
    }
}
