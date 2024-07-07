using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Autentica]
public class GetCarrinhoController : ControllerBase
{
    private readonly IGetCarrinhoService _getCarrinhoService;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public GetCarrinhoController(IGetCarrinhoService getCarrinhoService, IUsuarioAutenticado usuarioAutenticado)
    {
        _getCarrinhoService = getCarrinhoService;
        _usuarioAutenticado = usuarioAutenticado;
    }

    [HttpGet("get-carrinho")]
    public async Task<IActionResult> GetCarrinho()
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        var result = await _getCarrinhoService.GetCarrinhoAsync(new UsuarioViewModel()
        {
            Cnpj = usuario.Cnpj,
            Cpf = usuario.Cpf,
            DataDeAtualizacao = usuario.DataDeAtualizacao,
            DataDeCriacao = usuario.DataDeCriacao,
            Email = usuario.Email,
            Id = usuario.Id,
            Nome = usuario.Nome,
            Numero = usuario.Numero,
            Telefone = usuario.Telefone
        });
        return Ok(result);
    }
}
