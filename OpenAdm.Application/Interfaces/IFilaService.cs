using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Application.Interfaces;

public interface IFilaService
{
    Task PublicarAsync(EventoAplicacao evento);
}