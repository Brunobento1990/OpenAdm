using OpenAdm.Domain.Enuns;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Estoques;

public class MovimentacaoDeProdutoDto
{
    public Guid ProdutoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public Guid? PesoId { get; set; }
    public decimal Quantidade { get; set; }
    [MaxLength(255)]
    public string? Observacao { get; set; }
    public TipoMovimentacaoDeProduto TipoMovimentacaoDeProduto { get; set; }
}
