using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("carrinho")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CarrinhoController : ControllerBaseApi
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly IAddCarrinhoService _addCarrinhoSerice;

    public CarrinhoController(ICarrinhoService carrinhoService, IAddCarrinhoService addCarrinhoSerice)
    {
        _carrinhoService = carrinhoService;
        _addCarrinhoSerice = addCarrinhoSerice;
    }

    [HttpPut("adicionar")]
    public async Task<IActionResult> AdicionarCarinho(IList<AddCarrinhoModel> addCarrinhoDto)
    {
        try
        {
            await _addCarrinhoSerice.AddCarrinhoAsync(addCarrinhoDto);
            return Ok(new { message = "Produto adicionado com sucesso!" });
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("get-carrinho")]
    public async Task<IActionResult> GetCarrinho()
    {
        try
        {
            var result = await _carrinhoService.GetCarrinhoAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpGet("get-carrinho-count")]
    public async Task<IActionResult> GetCarrinhoCount()
    {
        try
        {
            var result = await _carrinhoService.GetCountCarrinhoAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpDelete("delete-produto-carrinho")]
    public async Task<IActionResult> DeleteProdutCarrinho([FromQuery] Guid produtoId)
    {
        try
        {
            var result = await _carrinhoService.DeleteProdutoCarrinhoAsync(produtoId);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
