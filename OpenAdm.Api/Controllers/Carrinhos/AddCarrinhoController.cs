using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Model.Carrinho;

namespace OpenAdm.Api.Controllers.Carrinhos;


[ApiController]
[Route("carrinho")]
[Autentica]
[AutenticaParceiro]
public class AddCarrinhoController : ControllerBase
{
    private readonly IAddCarrinhoService _addCarrinhoSerice;
    public AddCarrinhoController(
        IAddCarrinhoService addCarrinhoSerice)
    {
        _addCarrinhoSerice = addCarrinhoSerice;
    }

    [HttpPut("adicionar")]
    public async Task<IActionResult> AdicionarCarinho(IList<AddCarrinhoModel> addCarrinhoDto)
    {
        var quantidadeDeItens = await _addCarrinhoSerice.AddCarrinhoAsync(addCarrinhoDto);
        return Ok(quantidadeDeItens);
    }
}
