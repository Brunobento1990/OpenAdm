using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Worker.Infra.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly ParceiroContext _parceiroContext;

    public PedidoRepository(ParceiroContext parceiroContext)
    {
        _parceiroContext = parceiroContext;
    }

    public Task<Pedido> AddAsync(Pedido entity)
    {
        throw new NotImplementedException();
    }

    public Task<Pedido> AdicionarAsync(Pedido entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Pedido entity)
    {
        throw new NotImplementedException();
    }

    public Task<IList<StatusPedidoHomeModel>> GetCountStatusPedidosAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Pedido?> GetPedidoByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Pedido?> GetPedidoByUsuarioIdAsync(Guid usuarioId)
    {
        throw new NotImplementedException();
    }

    public async Task<Pedido?> GetPedidoCompletoByIdAsync(Guid id)
    {
        return await _parceiroContext
            .Pedidos
            .AsNoTracking()
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
    }

    public Task<IDictionary<Guid, Pedido>> GetPedidosAsync(IList<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Pedido>> GetPedidosByRelatorioPorPeriodoAsync(RelatorioPedidoDto relatorioPedidoDto)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Pedido>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Pedido>> GetPedidosEmAbertoAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> GetQuantidadeDePedidoPorUsuarioAsync(Guid usuarioId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetQuantidadePorStatusUsuarioAsync(Guid usuarioId, StatusPedido statusPedido)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetTotalPedidoPorUsuarioAsync(Guid usuarioId)
    {
        throw new NotImplementedException();
    }

    public Task<VariacaoMensalHome> ObterHomeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Pedido?> ObterPedidoParaCobrancaAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PaginacaoViewModel<Pedido>> PaginacaoAsync(FilterModel<Pedido> filterModel)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Pedido>> PaginacaoDropDownAsync(PaginacaoDropDown<Pedido> paginacaoDropDown)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public void Update(Pedido entity)
    {
        throw new NotImplementedException();
    }

    public Task<Pedido> UpdateAsync(Pedido entity)
    {
        throw new NotImplementedException();
    }
}
