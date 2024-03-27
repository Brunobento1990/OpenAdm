using OpenAdm.Application.Models.Carrinhos;

namespace OpenAdm.Application.Interfaces;

public interface ICarrinhoService
{
    Task<IList<CarrinhoViewModel>> GetCarrinhoAsync();
    Task<int> GetCountCarrinhoAsync();
    Task<bool> DeleteProdutoCarrinhoAsync(Guid produtoId);
    Task LimparCarrinhoDoUsuarioAsync(Guid usuarioId);
}
