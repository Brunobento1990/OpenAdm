using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Application.Interfaces.Ecommerce;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class ProdutoEcommerceService : IProdutoEcommerceService
{
    private readonly IProdutoEcommerceRepository _repository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public ProdutoEcommerceService(IProdutoEcommerceRepository repository, IUsuarioAutenticado usuarioAutenticado)
    {
        _repository = repository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<ResultadoProdutoEcommerceModel> ListarAsync(ProdutoEcommerceFiltroDto filtro)
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoOrNullAsync();

        var resultado = await _repository.ListarAsync(
            filtro.Search,
            filtro.Page,
            filtro.CategoriasIds);

        foreach (var produto in resultado.Produtos)
        {
            
        }

        return resultado;
    }
}