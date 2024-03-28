using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class GetCountCarrinhoService : IGetCountCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;

    public GetCountCarrinhoService(ICarrinhoRepository carrinhoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
    }

    public async Task<int> GetCountCarrinhoAsync(Guid usuarioId)
    {
        return await _carrinhoRepository.GetCountCarrinhoAsync(usuarioId.ToString());
    }
}
