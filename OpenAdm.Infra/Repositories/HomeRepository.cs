using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Infra.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly ParceiroContext _parceiroContext;

    public HomeRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public async Task<IList<StatusPedidoHomeModel>> ObterStatusPedidosAsync()
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .GroupBy(x => x.StatusPedido)
            .Select(x => new StatusPedidoHomeModel()
            {
                Status = x.Key,
                Quantidade = x.Count()
            })
            .OrderByDescending(y => y.Quantidade)
            .ToListAsync();
    }

    public async Task<int> CountDePedidosAsync()
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .CountAsync();
    }

    public async Task<TotalizadorProtudoEstoqueHome?> ObterTotalizadoProtudoEstoqueAsync()
    {
        return await _parceiroContext
            .Estoques
            .AsNoTracking()
            .Select(x => new
            {
                x.Quantidade,
                x.QuantidadeReservada
            })
            .GroupBy(_ => 1)
            .Select(g => new TotalizadorProtudoEstoqueHome
            {
                Quantidade = g.Sum(x => x.Quantidade),
                QuantidadeReservada = g.Sum(x => x.QuantidadeReservada)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IList<ContadorPedidoModel>> ContatorPedido7DiasAsync(DateTime dataInicio)
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
            .Where(p => p.DataDeCriacao >= dataInicio)
            .GroupBy(p => p.DataDeCriacao.Date)
            .Select(g => new ContadorPedidoModel()
            {
                Data = g.Key,
                Total = g.Count()
            })
            .ToListAsync();
    }
}