using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoEstoqueDto : FilterModel<Estoque>
{
    public Guid? ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }

    public override Expression<Func<Estoque, bool>>? Where()
    {
        if (!ProdutoId.HasValue && !PesoId.HasValue && !TamanhoId.HasValue)
        {
            return null;
        }

        Expression<Func<Estoque, bool>>? baseExpression = x => true;
        var filtros = new List<Expression<Func<Estoque, bool>>>();

        if (ProdutoId.HasValue)
        {
            filtros.Add(x => x.ProdutoId == ProdutoId.Value);
        }

        if (PesoId.HasValue)
        {
            filtros.Add(x => x.PesoId == PesoId.Value);
        }

        if (TamanhoId.HasValue)
        {
            filtros.Add(x => x.TamanhoId == TamanhoId.Value);
        }

        if (filtros.Count == 0 || baseExpression == null)
        {
            return null;
        }

        var combinedFilter = filtros[0];
        for (int i = 1; i < filtros.Count; i++)
        {
            combinedFilter = CombineExpressions(combinedFilter, filtros[i], Expression.OrElse);
        }

        return CombineExpressions(baseExpression, combinedFilter, Expression.AndAlso);
    }

    private static Expression<Func<Estoque, bool>> CombineExpressions(
    Expression<Func<Estoque, bool>> first,
    Expression<Func<Estoque, bool>> second,
    Func<Expression, Expression, BinaryExpression> combineOperator)
    {
        var parameter = Expression.Parameter(typeof(Estoque), "x");
        var firstBody = first.Body.ReplaceParameter(first.Parameters[0], parameter);
        var secondBody = second.Body.ReplaceParameter(second.Parameters[0], parameter);
        var combinedBody = combineOperator(firstBody, secondBody);
        return Expression.Lambda<Func<Estoque, bool>>(combinedBody, parameter);
    }

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

public static class ExpressionExtensions
{
    public static Expression ReplaceParameter(this Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        return new ParameterReplacer(oldParameter, newParameter).Visit(expression);
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
