using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ICobrancaPedidoService
{
    Task<ResultPartner<PagamentoViewModel>> CobrarAsync(GerarCobrancaPedidoDto gerarCobrancaPedidoDto);
}