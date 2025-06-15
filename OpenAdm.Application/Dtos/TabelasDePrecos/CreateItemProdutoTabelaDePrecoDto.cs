namespace OpenAdm.Application.Dtos.TabelasDePrecos;

public class CreateItemProdutoTabelaDePrecoDto
{
    public Guid? TamanhoId { get; set; }
    public Guid? PesoId { get; set; }
    public decimal ValorUnitarioAtacado { get; set; }
    public decimal ValorUnitarioVarejo { get; set; }
}
