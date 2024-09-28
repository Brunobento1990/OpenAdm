using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("item-tabela-de-preco")]
[Autentica]
[IsFuncionario]
[AutenticaParceiro]
public class ItemTabelaDePrecoController : ControllerBase
{
    private readonly IItemTabelaDePrecoService _itemTabelaDePrecoService;

    public ItemTabelaDePrecoController(IItemTabelaDePrecoService itemTabelaDePrecoService)
    {
        _itemTabelaDePrecoService = itemTabelaDePrecoService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateItemTabelaDePreco(CreateItensTabelaDePrecoDto createItensTabelaDePrecoDto)
    {
        await _itemTabelaDePrecoService.CreateItemTabelaDePrecoAsync(createItensTabelaDePrecoDto);
        return Ok();
    }

    [HttpPost("create-list")]
    public async Task<IActionResult> CreateListItemTabelaDePreco(IList<CreateItensTabelaDePrecoDto> createItensTabelaDePrecoDto)
    {
        await _itemTabelaDePrecoService.CreateListItemTabelaDePrecoAsync(createItensTabelaDePrecoDto);
        return Ok();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteItem([FromQuery] Guid id)
    {
        await _itemTabelaDePrecoService.DeleteItemAsync(id);
        return Ok();
    }
}
