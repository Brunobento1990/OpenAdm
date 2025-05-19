namespace OpenAdm.Application.Dtos.Categorias;

public class UpdateCategoriaDto : CategoriaCreateDto
{
    public Guid Id { get; set; }
    public bool InativoEcommerce { get; set; }
}
