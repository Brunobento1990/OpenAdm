using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Dtos.TabelasDePrecos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.TabelaDePrecos;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("item-tabela-de-preco")]
[Autentica]
[IsFuncionario]
[AcessoParceiro]
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

    [HttpGet("obter-itens")]
    [ProducesResponseType<IList<ItensTabelaDePrecoViewModel>>(200)]
    [ProducesResponseType<IList<ErrorResponse>>(400)]
    public async Task<IActionResult> ObterItens([FromQuery] Guid tabelaDePrecoId)
    {
        var response = await _itemTabelaDePrecoService.ObterItensDaTabelaDePrecoAsync(tabelaDePrecoId);
        return Ok(response);
    }
}
