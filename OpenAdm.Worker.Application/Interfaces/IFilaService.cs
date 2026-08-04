using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Worker.Application.Interfaces;

public interface IFilaService
{
    Task<IFilaConsumer> InscreverAsync(string fila);
    Task PublicarAsync(EventoAplicacao evento);
}
