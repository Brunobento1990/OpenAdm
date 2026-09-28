using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Representantes;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class HistoricoClienteRepresentanteService(IPedidoRepository pedidoRepository)
    : IHistoricoClienteRepresentanteService
{
    public async Task<HistoricoClienteRepresentanteViewModel> ObterAsync(
        Guid clienteId,
        Guid representanteId)
    {
        var historico = await pedidoRepository
            .ObterHistoricoClienteRepresentanteAsync(clienteId, representanteId);

        return new HistoricoClienteRepresentanteViewModel
        {
            UltimaCompra = FormatarUltimaCompra(historico.DataUltimaCompra),
            TicketMedio = $"Ticket médio: {historico.TicketMedio.FormatMoney(true)}",
            ProdutoMaisComprado = historico.ProdutoMaisComprado ?? "Nenhum produto comprado",
            UltimoPedido = $"Último pedido — {historico.ValorUltimoPedido.FormatMoney(true)}"
        };
    }

    private static string FormatarUltimaCompra(DateTime? data)
    {
        if (!data.HasValue)
        {
            return "Nenhuma compra realizada";
        }

        var quantidadeDias = Math.Max(0, (DateTime.UtcNow.Date - data.Value.Date).Days);
        var descricaoDias = quantidadeDias == 1 ? "há 1 dia" : $"há {quantidadeDias} dias";

        return $"Última compra: {data.Value.DateTimeSomenteDataToString()} {descricaoDias}";
    }
}
