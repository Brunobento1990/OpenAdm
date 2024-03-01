
namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class CreateItensTabelaDePrecoDto
{
    public Guid? Id { get; set; }
    public decimal ValorUnitario { get; set; }
    public Guid? TamanhoId { get; set; }
    public Guid ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
}
