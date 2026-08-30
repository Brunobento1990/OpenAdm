using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoTamanhoDto : FilterModel<Tamanho>
{
    public override Expression<Func<Tamanho, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            if (!ListarInativo)
            {
                return x => x.Ativo;
            }

            return null;
        }

        if (!ListarInativo)
        {
            return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{Search}%") && x.Ativo;
        }

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{Search}%");
    }
}
