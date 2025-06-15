using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Autentica]
[AcessoParceiro]
public class DeleteProdutoCarrinhoController : ControllerBase
{
    private readonly IDeleteProdutoCarrinhoService _deleteProdutoCarrinhoService;

    public DeleteProdutoCarrinhoController(
        IDeleteProdutoCarrinhoService deleteProdutoCarrinhoService)
    {
        _deleteProdutoCarrinhoService = deleteProdutoCarrinhoService;
    }

    [HttpDelete("delete-produto-carrinho")]
    public async Task<IActionResult> DeleteProdutCarrinho([FromQuery] Guid produtoId)
    {
        var result = await _deleteProdutoCarrinhoService.DeleteProdutoCarrinhoAsync(produtoId);
        return Ok(result);
    }
}
