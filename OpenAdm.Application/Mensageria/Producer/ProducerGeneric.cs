using OpenAdm.Application.Mensageria.Interfaces;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace OpenAdm.Application.Mensageria.Producer;

public class ProducerGeneric<T> : IProducerGeneric<T> where T : class
{

    private readonly IModel _channel;
    public ProducerGeneric(IModel channel)
    {
        _channel = channel;
    }
    public void Publish(T obj, string exchange)
    {
        _channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: false);
        var message = JsonSerializer.Serialize(obj);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: messageBytes);
    }
}
