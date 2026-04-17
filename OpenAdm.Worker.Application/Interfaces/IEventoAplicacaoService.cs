using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Model;
using OpenAdm.Worker.Application.DTOs;

namespace OpenAdm.Worker.Application.Interfaces;

public interface IEventoAplicacaoService
{
    Task<ResultPartner<ResultadoEventoAplicacaoDTO>> ExecutarAsync(EventoAplicacao eventoAplicacao);
}