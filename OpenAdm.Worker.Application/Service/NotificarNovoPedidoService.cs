using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Model;
using OpenAdm.Worker.Application.DTOs;
using OpenAdm.Worker.Application.Interfaces;

namespace OpenAdm.Worker.Application.Service;

public class NotificarNovoPedidoService : IEventoAplicacaoService
{
    public async Task<ResultPartner<ResultadoEventoAplicacaoDTO>> ExecutarAsync(EventoAplicacao eventoAplicacao)
    {
        return new ResultPartner<ResultadoEventoAplicacaoDTO>()
        {
            Result = new ResultadoEventoAplicacaoDTO()
            {
                Mensagem = "Ok"
            }
        };
    }
}