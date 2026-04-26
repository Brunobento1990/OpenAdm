namespace OpenAdm.Application.Dtos.Estoques;

public class UpdateEstoqueDto
{
    public Guid ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public decimal? Quantidade { get; set; }
}

public class ItemUpdateEstoquesDto
{
    public Guid Id { get; set; }
    public decimal? Quantidade { get; set; }
}

public class UpdateEstoquesDto
{
    public ICollection<ItemUpdateEstoquesDto> Dados { get; set; } = [];
}
