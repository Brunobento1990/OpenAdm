using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Interfaces.Carrinhos;

namespace OpenAdm.Api.Controllers.Carrinhos;

[ApiController]
[Route("carrinho")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class DeleteProdutoCarrinhoController : ControllerBaseApi
{
    private readonly IDeleteProdutoCarrinhoService _deleteProdutoCarrinhoService;
    private readonly Guid _usuarioId;

    public DeleteProdutoCarrinhoController(
        IDeleteProdutoCarrinhoService deleteProdutoCarrinhoService,
        ITokenService tokenService)
    {
        _usuarioId = tokenService.GetTokenUsuarioViewModel().Id;
        _deleteProdutoCarrinhoService = deleteProdutoCarrinhoService;
    }

    [HttpDelete("delete-produto-carrinho")]
    public async Task<IActionResult> DeleteProdutCarrinho([FromQuery] Guid produtoId)
    {
        try
        {
            var result = await _deleteProdutoCarrinhoService.DeleteProdutoCarrinhoAsync(produtoId, _usuarioId);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
