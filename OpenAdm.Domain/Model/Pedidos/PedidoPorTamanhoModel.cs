using OpenAdm.Domain.Validations;

namespace OpenAdm.Domain.Model.Pedidos;

public class PedidoPorTamanhoModel
{
    public PedidoPorTamanhoModel(Guid produtoId, Guid tamanhoId, decimal quantidade)
    {
        ValidationGuid.ValidGuidNullAndEmpty(produtoId);
        ValidationGuid.ValidGuidNullAndEmpty(tamanhoId);
        ValidationDecimal.ValidDecimalNullAndZero(quantidade);

        ProdutoId = produtoId;
        TamanhoId = tamanhoId;
        Quantidade = quantidade;
    }

    public Guid ProdutoId { get; private set; }
    public Guid TamanhoId { get; private set; }
    public decimal Quantidade { get; private set; }
}
