using Microsoft.EntityFrameworkCore;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Application.Queries;
using OpenAdm.Data.Context;

namespace OpenAdm.Infra.QueryService;

public class CobrancaPedidoQueryService : ICobrancaPedidoQueryService
{
    private readonly ParceiroContext _context;

    public CobrancaPedidoQueryService(ParceiroContext context)
    {
        _context = context;
    }

    public async Task<PedidoCobrancaQuery?> ObterAsync(Guid pedidoId, Guid usuarioId)
    {
        return await _context
            .Pedidos
            .AsNoTracking()
            .Where(pedido => pedido.Id == pedidoId && pedido.UsuarioId == usuarioId)
            .Select(x => new PedidoCobrancaQuery()
            {
                Valor = x.ItensPedido.Sum(y => y.Quantidade * y.ValorUnitario),
                ValorFrete = x.EnderecoEntrega!.ValorFrete,
                Cliente = x.Usuario.Nome,
                NumeroPedido = x.Numero,
                PedidoId = x.Id,
            })
            .FirstOrDefaultAsync();
    }
}