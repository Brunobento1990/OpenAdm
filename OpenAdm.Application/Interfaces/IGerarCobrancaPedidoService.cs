using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IGerarCobrancaPedidoService
{
    Task<ResultPartner<PagamentoViewModel>> GerarCobrancaAsync(Pedido pedido, string urlWebHook, string acessToken);
}