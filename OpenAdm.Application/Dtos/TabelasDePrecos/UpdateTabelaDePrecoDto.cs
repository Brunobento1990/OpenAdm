using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class UpdateTabelaDePrecoDto
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;
    public bool AtivaEcommerce { get; set; }
}
