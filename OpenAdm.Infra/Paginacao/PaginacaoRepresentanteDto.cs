using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Infra.Paginacao;

public sealed class PaginacaoRepresentanteDto : FilterModel<Representante>
{
    public override Expression<Func<Representante, bool>>? GetWhereBySearch()
    {
        var search = Search?.Trim();
        if (string.IsNullOrWhiteSpace(search))
            return ListarInativo ? null : x => x.Ativo;

        return ListarInativo
            ? x => EF.Functions.ILike(EF.Functions.Unaccent(x.Nome), $"%{search}%") ||
                   (x.Cpf != null && x.Cpf.Contains(search)) ||
                   (x.Email != null && EF.Functions.ILike(x.Email, $"%{search}%"))
            : x => x.Ativo && (EF.Functions.ILike(EF.Functions.Unaccent(x.Nome), $"%{search}%") ||
                               (x.Cpf != null && x.Cpf.Contains(search)) ||
                               (x.Email != null && EF.Functions.ILike(x.Email, $"%{search}%")));
    }
}
