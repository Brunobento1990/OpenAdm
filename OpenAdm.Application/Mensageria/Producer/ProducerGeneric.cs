using OpenAdm.Application.Mensageria.Interfaces;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using OpenAdm.Domain.Factories.Interfaces;
using System.Threading.Channels;

namespace OpenAdm.Application.Mensageria.Producer;

public class ProducerGeneric<T> : IProducerGeneric<T> where T : class
{

    private readonly IModel _channel;
    private readonly string _referer;
    public ProducerGeneric(IModel channel, IDomainFactory domainFactory)
    {
        _referer = domainFactory.GetDomainParceiro();
        _channel = channel;
    }
    public void Publish(T obj, string exchange)
    {
        var header = new Dictionary<string, object>();
        header.TryAdd("Referer", _referer);
        _channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: false, autoDelete: false);
        var message = JsonSerializer.Serialize(obj);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var basicProperties = _channel.CreateBasicProperties();
        basicProperties.Headers = header;
        _channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: basicProperties, body: messageBytes);
    }
}
