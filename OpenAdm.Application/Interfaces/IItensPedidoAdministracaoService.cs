using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IItensPedidoAdministracaoService
{
    Task<ResultPartner<ItensPedidoViewModel>> AdicionarAsync(AddItemPedidoDto dto);
    Task<ResultPartner<ItensPedidoViewModel>> EditarQuantidadeAsync(UpdateQuantidadeItemPedidoDto dto);
    Task<ResultPartner<ResultadoPadraoViewModel>> InativarAsync(Guid id);
}
