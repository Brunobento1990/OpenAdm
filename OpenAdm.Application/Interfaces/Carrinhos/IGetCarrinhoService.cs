using OpenAdm.Application.Models.Carrinhos;

namespace OpenAdm.Application.Interfaces.Carrinhos;

public interface IGetCarrinhoService
{
    Task<CarrinhoViewModel> GetCarrinhoAsync();
    Task<CarrinhoViewModelV2> GetCarrinhoV2Async();
}
