using RabbitMQ.Client;

namespace OpenAdm.Application.Mensageria;

public static class ConfiguracaoBase
{
    public static IConnection InitConnection(string uri)
    {
        ConnectionFactory factory = new()
        {
            Uri = new Uri(uri)
        };

        return factory.CreateConnection();
    }
}
