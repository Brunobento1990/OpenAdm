namespace OpenAdm.Application.Models.Estoques;

public class PosicaoDeEstoqueRelatorioViewModel
{
    public ICollection<EstoqueViewModel> Itens { get; set; } = [];
}