using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Application.Models.CobrancasPedidosEcommerce;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ICobrancaPedidoService
{
    Task<ResultPartner<CobrancaPedidoViewModel>> GetParaNegociacaoAsync(Guid pedidoId);
    Task<ResultPartner<PagamentoViewModel>> CobrarAsync(GerarCobrancaPedidoDto gerarCobrancaPedidoDto);
}
