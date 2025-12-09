namespace OpenAdm.Application.Dtos.Estoques;

public class PosicaoDeEstoqueRelatorioDto
{
    public ICollection<Guid>? Produtos { get; set; }
    public ICollection<Guid>? Tamanhos { get; set; }
    public ICollection<Guid>? Pesos { get; set; }
    public ICollection<Guid>? Categorias { get; set; }
}