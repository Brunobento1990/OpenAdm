namespace OpenAdm.Domain.PaginateDto;

public class PaginacaoProdutoEcommerceDto
{
    public int Page { get; set; }
    public Guid? CategoriaId { get; set; }
    public Guid? TamanhoId { get; set; }
    public Guid? PesoId { get; set; }
}
