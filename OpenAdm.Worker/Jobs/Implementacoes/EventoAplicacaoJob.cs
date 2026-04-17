using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Worker.Application.Interfaces;
using OpenAdm.Worker.Application.Service;
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

        var evento = await eventoRepository.ProximoEventoAsync();

        if (evento == null)
        {
            return;
        }

        var parceiroAutenticado = scope.ServiceProvider.GetRequiredService<IParceiroAutenticado>();


        parceiroAutenticado.ConnectionString = Criptografia.Decrypt(evento.EmpresaOpenAdm.ConnectionString);
        parceiroAutenticado.Id = evento.EmpresaOpenAdmId;

        if (!evento.PodeExecutar)
        {
            return;
        }

        try
        {
            var eventoServico = scope.ServiceProvider.GetKeyedService<IEventoAplicacaoService>(evento.TipoEventoAplicacao);

            if (eventoServico == null)
            {
                evento.AdicionarMensagem($"Servico null para o evento: {evento.TipoEventoAplicacao}");
                evento.AdicionarTentativa();
                await eventoRepository.SaveChangesAsync();
                LogService.Info($"Evento sem implementação: {evento.TipoEventoAplicacao}");
                return;
            }

            var resultado = await eventoServico.ExecutarAsync(evento);

            if (!string.IsNullOrWhiteSpace(resultado.Error))
            {
                LogService.Error($"Erro no evento: {evento.TipoEventoAplicacao}");
                evento.AdicionarMensagem(resultado.Error);
                evento.AdicionarTentativa();
                await eventoRepository.SaveChangesAsync();
                return;
            }

            evento.Finalizada(resultado.Result?.Mensagem);
            await eventoRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            LogService.Error($"Erro no evento: {ex.InnerException?.Message ?? ex.Message}");
            evento.AdicionarMensagem(ex.InnerException?.Message ?? ex.Message);
            evento.AdicionarTentativa();
            await eventoRepository.SaveChangesAsync();
        }
    }

    public static string Key => "EventoAplicacaoJobKey";
    public static string IdentityTrigger => "EventoAplicacaoJobIdentityTrigger";

    public static string ObterConfiguracaoTempoDeExecucao(IConfiguration configuracao)
    {
        var cron = configuracao["Jobs:EventoAplicacao"];
        return string.IsNullOrWhiteSpace(cron) ? "0 */1 * * * ?" : cron;
    }
}
