using OpenAdm.Worker.Application.Interfaces;
using StackExchange.Redis;

namespace OpenAdm.Worker.Infra.Services;

public class FilaConsumer : IFilaConsumer
{
    private readonly IDatabase _db;

    private readonly string _fila;

    private readonly string _grupo;

    private readonly string _consumer;

    public FilaConsumer(
        IDatabase db,
        string fila,
        string grupo,
        string consumer)
    {
        _db = db;
        _fila = fila;
        _grupo = grupo;
        _consumer = consumer;
    }

    public async Task InicializarAsync()
    {
        try
        {
            await _db.StreamCreateConsumerGroupAsync(
                _fila,
                _grupo,
                "0-0",
                createStream: true);
        }
        catch (RedisServerException ex)
            when (ex.Message.StartsWith("BUSYGROUP"))
        {
            // grupo já existe
        }
    }

    public async Task<MensagemFila?> LerAsync(
        CancellationToken cancellationToken)
    {
        var mensagens = await _db.StreamReadGroupAsync(
            _fila,
            _grupo,
            _consumer,
            ">",
            count: 1);

        await Task.Delay(2000, cancellationToken);

        if (mensagens.Length == 0)
        {
            return null;
        }

        var mensagem = mensagens[0];

        return new MensagemFila
        {
            Id = mensagem.Id!,
            Conteudo = mensagem.Values
                .First(x => x.Name == "data")
                .Value!
        };
    }

    public Task ConfirmarAsync(string id)
    {
        return _db.StreamAcknowledgeAsync(
            _fila,
            _grupo,
            id);
    }
}