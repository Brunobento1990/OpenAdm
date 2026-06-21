namespace OpenAdm.Application.Dtos.Produtos;

public class ProdutoEcommerceFiltroDto
{
    public int Page { get; set; }
    public string? Search { get; set; }
    public ICollection<Guid>? CategoriasIds { get; set; }
    public ICollection<Guid>? PesosIds { get; set; }
    public ICollection<Guid>? TamanhosIds { get; set; }
}