using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Infra.Paginacao;

public sealed class PaginacaoItemCatalogoRepresentanteDto : FilterModel<ItemTabelaDePreco>
{
    public Guid TabelaDePrecoId { get; set; }

    public override Expression<Func<ItemTabelaDePreco, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        var search = Search.RemoverAcentos();
        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Produto.Descricao), $"%{search}%");
    }

    public override Expression<Func<ItemTabelaDePreco, bool>> Where() =>
        x => x.TabelaDePrecoId == TabelaDePrecoId &&
             x.Produto.Ativo &&
             (x.PesoId == null || x.Peso!.Ativo) &&
             (x.TamanhoId == null || x.Tamanho!.Ativo);

    public override IList<Expression<Func<ItemTabelaDePreco, object>>> IncludeCustomList() =>
    [
        x => x.Produto,
        x => x.Peso!,
        x => x.Tamanho!
    ];
}
