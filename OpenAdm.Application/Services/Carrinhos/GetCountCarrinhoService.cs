using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services.Carrinhos;

public sealed class GetCountCarrinhoService : IGetCountCarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public GetCountCarrinhoService(ICarrinhoRepository carrinhoRepository, IUsuarioAutenticado usuarioAutenticado)
    {
        _carrinhoRepository = carrinhoRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<int> GetCountCarrinhoAsync()
    {
        return await _carrinhoRepository.GetCountCarrinhoAsync(_usuarioAutenticado.Id.ToString());
    }
}
