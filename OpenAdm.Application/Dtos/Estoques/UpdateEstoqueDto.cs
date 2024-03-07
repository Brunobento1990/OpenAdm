using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Estoques;

public class UpdateEstoqueDto
{
    [Required]
    public Guid ProdutoId { get; set; }
    [Required]
    public decimal Quantidade { get; set; }
}
