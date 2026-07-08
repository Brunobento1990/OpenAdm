namespace OpenAdm.Worker.Application.Interfaces;

public interface IFilaService
{
    Task<IFilaConsumer> InscreverAsync(string fila);
}