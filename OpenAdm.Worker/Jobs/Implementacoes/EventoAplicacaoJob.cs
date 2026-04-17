using OpenAdm.Domain.Interfaces;
using OpenAdm.Worker.Application.Interfaces;
using OpenAdm.Worker.Jobs.Interfaces;

namespace OpenAdm.Worker.Jobs.Implementacoes;

public class EventoAplicacaoJob : BaseJob, IJobInfo
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public EventoAplicacaoJob(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public override async Task ExecuteTask()
    {
        if (_configuration["Jobs:EventoAplicacaoInativo"]?.ToLower() == "true")
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();

        var eventoRepository = scope.ServiceProvider.GetRequiredService<IEventoAplicacaoRepository>();
        var eventos = await eventoRepository.ProximosEventosAsync();

        foreach (var evento in eventos)
        {
            var eventoServico =
                scope.ServiceProvider.GetKeyedService<IEventoAplicacaoService>(evento.TipoEventoAplicacao);

            if (eventoServico == null)
            {
                evento.AdicionarMensagem($"Servico null para o evento: {evento.TipoEventoAplicacao}");
                evento.AdicionarTentativa();
                await eventoRepository.SaveChangesAsync();
                continue;
            }

            var resultado = await eventoServico.ExecutarAsync(evento);

            if (!string.IsNullOrWhiteSpace(resultado.Error))
            {
                evento.AdicionarMensagem($"Servico null para o evento: {evento.TipoEventoAplicacao}");
                evento.AdicionarTentativa();
                await eventoRepository.SaveChangesAsync();
                continue;
            }

            evento.Finalizada(resultado.Result?.Mensagem);
            await eventoRepository.SaveChangesAsync();
        }
    }

    public static string Key => "EventoAplicacaoJobKey";
    public static string IdentityTrigger => "ConexaoWhatsAppJobIdentityTrigger";

    public static string ObterConfiguracaoTempoDeExecucao(IConfiguration configuracao)
    {
        var cron = configuracao["Jobs:EventoAplicacao"];
        return string.IsNullOrWhiteSpace(cron) ? "0 */1 * * * ?" : cron;
    }
}