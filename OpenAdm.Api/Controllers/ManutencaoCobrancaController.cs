using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Controllers;

[ApiController]
[Route("manutencao/cobranca")]
public sealed class ManutencaoCobrancaController(
    IEmpresaOpenAdmRepository empresaOpenAdmRepository,
    IParceiroAutenticado parceiroAutenticado,
    ParceiroContext parceiroContext) : ControllerBase
{
    [HttpPost("retroativa")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GerarCobrancasRetroativasAsync([FromQuery] Guid empresaId)
    {
        var empresa = await empresaOpenAdmRepository.ObterPorIdAsync(empresaId);
        if (empresa == null)
        {
            return BadRequest(new ErrorResponse
            {
                Mensagem = "Não foi possível localizar a empresa informada"
            });
        }

        parceiroAutenticado.Id = empresa.Id;
        parceiroAutenticado.ConnectionString = Criptografia.Decrypt(empresa.ConnectionString);

        var pedidosSemCobranca = await parceiroContext.Pedidos
            .AsNoTracking()
            .Where(pedido => pedido.StatusPedido != StatusPedido.Cancelado && pedido.Cobranca == null)
            .Include(pedido => pedido.ItensPedido)
            .Include(pedido => pedido.EnderecoEntrega)
            .Include(pedido => pedido.Fatura)
            .ToListAsync();

        var cobrancas = pedidosSemCobranca.Select(pedido => new CobrancaPedidoEcommerce(
            id: Guid.NewGuid(),
            dataDeCriacao: pedido.DataDeCriacao,
            dataDeAtualizacao: pedido.DataDeAtualizacao,
            numero: 0,
            pedidoId: pedido.Id,
            ativo: true,
            total: pedido.ValorTotalCobrar,
            status: pedido.Fatura != null
                ? StatusCobrancaPedidoEcommerceEnum.GeradoFatura
                : StatusCobrancaPedidoEcommerceEnum.ACobrar)).ToList();

        if (cobrancas.Count > 0)
        {
            await parceiroContext.CobrancasPedidosEcommerce.AddRangeAsync(cobrancas);
            await parceiroContext.SaveChangesAsync();
        }

        return Ok(new
        {
            EmpresaId = empresa.Id,
            CobrancasCriadas = cobrancas.Count,
            CobrancasComFatura = cobrancas.Count(x => x.Status == StatusCobrancaPedidoEcommerceEnum.GeradoFatura),
            CobrancasACobrar = cobrancas.Count(x => x.Status == StatusCobrancaPedidoEcommerceEnum.ACobrar)
        });
    }
}
