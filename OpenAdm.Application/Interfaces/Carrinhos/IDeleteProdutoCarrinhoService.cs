namespace OpenAdm.Application.Interfaces.Carrinhos;

public interface IDeleteProdutoCarrinhoService
{
    Task<bool> DeleteProdutoCarrinhoAsync(Guid produtoId, Guid usuarioId);
}
