using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Api.Controllers.Carrinhos;


[ApiController]
[Route("carrinho")]
[Autentica]
public class AddCarrinhoController : ControllerBase
{
    private readonly IAddCarrinhoService _addCarrinhoSerice;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    public AddCarrinhoController(
        IAddCarrinhoService addCarrinhoSerice,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _addCarrinhoSerice = addCarrinhoSerice;
        _usuarioAutenticado = usuarioAutenticado;
    }

    [HttpPut("adicionar")]
    public async Task<IActionResult> AdicionarCarinho(IList<AddCarrinhoModel> addCarrinhoDto)
    {
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();
        await _addCarrinhoSerice.AddCarrinhoAsync(addCarrinhoDto, new UsuarioViewModel()
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
        return Ok(new { message = "Produto adicionado com sucesso!" });
    }
}
