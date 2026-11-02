using Microsoft.AspNetCore.Mvc;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Controllers.Pedidos;

[ApiController]
[Route("pedidos/itens")]
[Autentica]
[IsFuncionario]
[AcessoParceiro]
public sealed class ItensPedidoAdministracaoController(IItensPedidoAdministracaoService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Adicionar(AddItemPedidoDto dto)
        => (await service.AdicionarAsync(dto)).ToActionResult();

    [HttpPut("quantidade")]
    public async Task<IActionResult> EditarQuantidade(UpdateQuantidadeItemPedidoDto dto)
        => (await service.EditarQuantidadeAsync(dto)).ToActionResult();

    [HttpDelete]
    public async Task<IActionResult> Inativar([FromQuery] Guid id)
    {
        return (await service.InativarAsync(id)).ToActionResult();
    }
}
