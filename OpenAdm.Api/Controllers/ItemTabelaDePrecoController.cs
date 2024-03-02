using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("item-tabela-de-preco")]
[Authorize(AuthenticationSchemes = "Bearer")]
[IsFuncionario]
public class ItemTabelaDePrecoController : ControllerBaseApi
{
    private readonly IItemTabelaDePrecoService _itemTabelaDePrecoService;

    public ItemTabelaDePrecoController(IItemTabelaDePrecoService itemTabelaDePrecoService)
    {
        _itemTabelaDePrecoService = itemTabelaDePrecoService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateItemTabelaDePreco(CreateItensTabelaDePrecoDto createItensTabelaDePrecoDto)
    {
        try
        {
            await _itemTabelaDePrecoService.CreateItemTabelaDePrecoAsync(createItensTabelaDePrecoDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteItem([FromQuery] Guid id)
    {
        try
        {
            await _itemTabelaDePrecoService.DeleteItemAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return await HandleErrorAsync(ex);
        }
    }
}
