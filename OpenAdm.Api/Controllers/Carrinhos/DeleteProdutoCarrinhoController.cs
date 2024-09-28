using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Interfaces.Carrinhos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Autentica]
[AutenticaParceiro]
public class DeleteProdutoCarrinhoController : ControllerBase
{
    private readonly IDeleteProdutoCarrinhoService _deleteProdutoCarrinhoService;
    private readonly Guid _usuarioId;

    public DeleteProdutoCarrinhoController(
        IDeleteProdutoCarrinhoService deleteProdutoCarrinhoService,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _usuarioId = usuarioAutenticado.Id;
        _deleteProdutoCarrinhoService = deleteProdutoCarrinhoService;
    }

    [HttpDelete("delete-produto-carrinho")]
    public async Task<IActionResult> DeleteProdutCarrinho([FromQuery] Guid produtoId)
    {
        var result = await _deleteProdutoCarrinhoService.DeleteProdutoCarrinhoAsync(produtoId, _usuarioId);
        return Ok();
    }
}
