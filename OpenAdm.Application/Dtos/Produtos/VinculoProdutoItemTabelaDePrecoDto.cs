namespace OpenAdm.Application.Dtos.Produtos;

public class VinculoProdutoItemTabelaDePrecoDto
{
    public decimal ValorUnitarioAtacado { get; set; }
    public decimal ValorUnitarioVarejo { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }
}

public class VinculoProdutoTabelaDePrecoDto
{
    public Guid TabelaDePrecoId { get; set; }
    public IList<VinculoProdutoItemTabelaDePrecoDto> Itens { get; set; } = new List<VinculoProdutoItemTabelaDePrecoDto>();
}
