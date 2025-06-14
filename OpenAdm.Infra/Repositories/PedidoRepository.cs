using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Extensions.IQueryable;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Infra.Repositories;

public class PedidoRepository(ParceiroContext parceiroContext)
    : GenericRepository<Pedido>(parceiroContext), IPedidoRepository
{

    public override async Task<PaginacaoViewModel<Pedido>> PaginacaoAsync(FilterModel<Pedido> filterModel)
    {
        var include = filterModel.IncludeCustom();
        var select = filterModel.SelectCustom();

        var query = ParceiroContext
            .Pedidos
            .AsNoTracking()
            .WhereIsNotNull(filterModel.GetWhereBySearch())
            .Include(x => x.ItensPedido)
            .Include(x => x.Usuario);

        var (TotalPaginas, Values) = await query
            .CustomFilterAsync(filterModel);

        var totalDeRegistros = await ParceiroContext.Pedidos.WhereIsNotNull(filterModel.GetWhereBySearch()).CountAsync();

        return new()
        {
            TotalPaginas = TotalPaginas,
            Values = Values,
            TotalDeRegistros = totalDeRegistros
        };
    }
    public async Task<int> GetCountPedidosEmAbertoAsync()
    {
        try
        {
            return await ParceiroContext
                .Pedidos
                .AsNoTracking()
                .Where(x => x.StatusPedido == StatusPedido.Aberto)
                .CountAsync();
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task<Pedido?> GetPedidoByIdAsync(Guid id)
    {
        return await ParceiroContext
            .Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(x => x.Usuario)
            .Include(x => x.ItensPedido)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id)
    {
        var pedido = await ParceiroContext
            .Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Produto)
                    .ThenInclude(x => x.Categoria)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Tamanho)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Peso)
            .Include(x => x.Usuario)
            .Include(x => x.EnderecoEntrega)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (pedido != null)
        {
            foreach (var item in pedido.ItensPedido)
            {
                if (item.Pedido != null)
                    item.Pedido = null!;

                if (item.Produto != null)
                {
                    item.Produto.Tamanhos = [];
                    item.Produto.Pesos = [];
                    item.Produto.ItensPedido = [];
                    item.Produto.ItensTabelaDePreco = [];
                    if (item.Produto.Categoria != null)
                    {
                        item.Produto.Categoria.Produtos = [];
                    }
                }

                if (item.Tamanho != null)
                {
                    item.Tamanho.ItensPedido = [];
                }

                if (item.Peso != null)
                {
                    item.Peso.ItensPedido = [];
                }
            }
        }

        return pedido;
    }

    public async Task<IDictionary<Guid, Pedido>> GetPedidosAsync(IList<Guid> ids)
    {
        return await ParceiroContext
            .Pedidos
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x);
    }

    public async Task<IList<Pedido>> GetPedidosByRelatorioPorPeriodoAsync(RelatorioPedidoDto relatorioPedidoDto)
    {
        return await ParceiroContext
            .Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .OrderByDescending(x => x.DataDeCriacao)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Produto)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Tamanho)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Peso)
            .Include(x => x.Usuario)
            .Where(x => x.DataDeCriacao.Date <= relatorioPedidoDto.DataFinal.Date &&
                x.DataDeCriacao.Date >= relatorioPedidoDto.DataInicial.Date &&
                x.StatusPedido == StatusPedido.Entregue)
            .WhereIsNotNull(relatorioPedidoDto.WhereUsuarioId())
            .ToListAsync();
    }

    public async Task<IList<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido)
    {
        return await ParceiroContext.Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(x => x.ItensPedido)
            .OrderByDescending(x => x.Numero)
            .AsQueryable()
            .Where(x => x.UsuarioId == usuarioId && x.StatusPedido == (StatusPedido)statusPedido)
            .ToListAsync();
    }

    public async Task<IList<Pedido>> GetPedidosEmAbertoAsync()
    {
        return await ParceiroContext
            .Pedidos
            .AsNoTracking()
            .Include(x => x.Usuario)
            .Include(x => x.ItensPedido)
            .Where(x => x.StatusPedido == StatusPedido.Aberto)
            .ToListAsync();
    }

    public async Task<int> GetQuantidadeDePedidoPorUsuarioAsync(Guid usuarioId)
    {
        return await ParceiroContext
            .Pedidos
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.UsuarioId == usuarioId)
            .CountAsync();
    }

    public async Task<int> GetQuantidadePorStatusUsuarioAsync(Guid usuarioId, StatusPedido statusPedido)
    {
        try
        {
            return await ParceiroContext
                .Pedidos
                .AsNoTracking()
                .CountAsync(x => x.StatusPedido == statusPedido && x.UsuarioId == usuarioId);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task<decimal> GetTotalPedidoPorUsuarioAsync(Guid usuarioId)
    {
        var pedidos = await ParceiroContext
            .Pedidos
            .AsNoTracking()
            .Include(x => x.ItensPedido)
            .Where(x => x.UsuarioId == usuarioId)
            .ToListAsync();

        return pedidos.Sum(x => x.ValorTotal);
    }

    public async Task<Pedido?> GetPedidoParaGerarPixByIdAsync(Guid id)
    {
        return await
            ParceiroContext
            .Pedidos
            .AsNoTracking()
            .Include(x => x.ItensPedido)
            .Include(x => x.Fatura!.Parcelas)
                .ThenInclude(x => x.Transacoes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<VariacaoMensalHome> ObterHomeAsync()
    {
        var hoje = DateTime.Today;
        var mes = hoje.Month;
        var anoAtual = hoje.Year;
        var anoAnterior = anoAtual - 1;

        // Obter totais de cada ano
        var totais = await ParceiroContext
            .ItensPedidos
            .AsNoTracking()
            .Where(i =>
                i.Pedido.DataDeCriacao.Month == mes &&
                (i.Pedido.DataDeCriacao.Year == anoAtual || i.Pedido.DataDeCriacao.Year == anoAnterior)
            )
            .GroupBy(i => i.Pedido.DataDeCriacao.Year)
            .Select(g => new
            {
                Ano = g.Key,
                Total = g.Sum(i => i.Quantidade)
            })
            .ToListAsync();

        // Extrair os valores
        var totalAnoAtual = totais.FirstOrDefault(x => x.Ano == anoAtual)?.Total ?? 0;
        var totalAnoAnterior = totais.FirstOrDefault(x => x.Ano == anoAnterior)?.Total ?? 0;

        // Calcular variação em porcentagem
        var variacao = totalAnoAnterior == 0
            ? (totalAnoAtual > 0 ? 100 : 0)
            : ((totalAnoAtual - totalAnoAnterior) / totalAnoAnterior) * 100;

        return new VariacaoMensalHome()
        {
            Mes = mes,
            TotalAnoAnterior = totalAnoAnterior,
            TotalAnoAtual = totalAnoAtual,
            Porcentagem = Math.Round(variacao, 2),
            AnoAnterior = anoAnterior,
            AnoAtual = anoAtual
        };
    }
}
