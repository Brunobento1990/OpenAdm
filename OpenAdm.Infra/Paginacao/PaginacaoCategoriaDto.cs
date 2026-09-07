using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Model;
using System.Linq.Expressions;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Infra.Paginacao;

public class PaginacaoCategoriaDto : FilterModel<Categoria>
{
    public override Expression<Func<Categoria, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
        {
            if (!ListarInativo)
            {
                return x => x.InativoEcommerce == false;
            }

            return null;
        }
        
        if (!ListarInativo)
        {
            return x => x.InativoEcommerce == false &&
                        EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{Search}%");
        }

        return x => EF.Functions.ILike(EF.Functions.Unaccent(x.Descricao), $"%{Search}%");
    }
}