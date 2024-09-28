using OpenAdm.Domain.Entities;
using OpenAdm.Application.Dtos.Categorias;

namespace OpenAdm.Application.Mappers;

public partial class EntityMapper
{
    public static Categoria ToCategoriaCreate(CategoriaCreateDto categoriaCreateDto, string? nomeFoto)
    {
        var date = DateTime.Now;
        return new Categoria(
            id: Guid.NewGuid(),
            dataDeCriacao: date,
            dataDeAtualizacao: date,
            numero: 0,
            descricao: categoriaCreateDto.Descricao,
            foto: categoriaCreateDto.Foto,
            nomeFoto: nomeFoto);
    }
}
