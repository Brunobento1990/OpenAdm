using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Api.Controllers.Carrinhos;


[ApiController]
[Route("carrinho")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class AddCarrinhoController : ControllerBaseApi
{
    private readonly IAddCarrinhoService _addCarrinhoSerice;
    private readonly ITokenService _tokenService;

    public AddCarrinhoController(IAddCarrinhoService addCarrinhoSerice, ITokenService tokenService)
    {
        _addCarrinhoSerice = addCarrinhoSerice;
        _tokenService = tokenService;
    }

    [HttpPut("adicionar")]
    public async Task<IActionResult> AdicionarCarinho(IList<AddCarrinhoModel> addCarrinhoDto)
    {
        try
        {
            var usuarioViewModel = _tokenService.GetTokenUsuarioViewModel();
            await _addCarrinhoSerice.AddCarrinhoAsync(addCarrinhoDto, usuarioViewModel);
            return Ok(new { message = "Produto adicionado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
