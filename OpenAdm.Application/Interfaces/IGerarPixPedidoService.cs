using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Models.Pagamentos;

namespace OpenAdm.Application.Interfaces;

public interface IGerarPixPedidoService
{
    Task<PagamentoViewModel> GerarPixAsync(GerarPixParcelaDto gerarPixParcelaDto);
}
