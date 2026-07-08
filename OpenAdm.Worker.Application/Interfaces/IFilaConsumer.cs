namespace OpenAdm.Worker.Application.Interfaces;

public interface IFilaConsumer
{
    Task<MensagemFila?> LerAsync(CancellationToken cancellationToken);

    Task ConfirmarAsync(string id);
}

public sealed class MensagemFila
{
    public required string Id { get; init; }

    public required string Conteudo { get; init; }
}