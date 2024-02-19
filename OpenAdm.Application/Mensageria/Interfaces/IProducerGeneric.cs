namespace OpenAdm.Application.Mensageria.Interfaces;

public interface IProducerGeneric<T> where T : class
{
    void Publish(T obj, string exchange);
}
