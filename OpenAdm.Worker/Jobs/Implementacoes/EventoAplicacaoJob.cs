using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Worker.Application.Interfaces;
using OpenAdm.Worker.Application.Service;

namespace OpenAdm.Worker.Jobs.Implementacoes;

public class EventoAplicacaoJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly int _delayEmMinutos;

    public EventoAplicacaoJob(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _delayEmMinutos = int.TryParse(configuration["Jobs:EventoAplicacao"], out var delay) ? delay : 5;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(_delayEmMinutos), stoppingToken);

            if (DateTime.Now.EhMadrugada())
            {
                LogService.Info("EventoAplicação: Madrugada");
                continue;
            }

            if (_configuration["Jobs:EventoAplicacaoInativo"]?.ToLower() == "true")
            {
                continue;
            }

            using var scope = _serviceProvider.CreateScope();
            var eventoRepository = scope.ServiceProvider.GetRequiredService<IEventoAplicacaoRepository>();

            var evento = await eventoRepository.ProximoEventoAsync();

            if (evento == null || !evento.PodeExecutar)
            {
                continue;
            }

            var parceiroAutenticado = scope.ServiceProvider.GetRequiredService<IParceiroAutenticado>();

            parceiroAutenticado.ConnectionString = Criptografia.Decrypt(evento.EmpresaOpenAdm.ConnectionString);
            parceiroAutenticado.Id = evento.EmpresaOpenAdmId;

            try
            {
                var eventoServico =
                    scope.ServiceProvider.GetKeyedService<IEventoAplicacaoService>(evento.TipoEventoAplicacao);

                if (eventoServico == null)
                {
                    evento.AdicionarMensagem($"Servico null para o evento: {evento.TipoEventoAplicacao}");
                    evento.AdicionarTentativa();
                    await eventoRepository.SaveChangesAsync();
                    LogService.Info($"Evento sem implementação: {evento.TipoEventoAplicacao}");
                    continue;
                }

                var resultado = await eventoServico.ExecutarAsync(evento);

                if (!string.IsNullOrWhiteSpace(resultado.Error))
                {
                    LogService.Error($"Erro no evento: {evento.TipoEventoAplicacao}");
                    evento.AdicionarMensagem(resultado.Error);
                    evento.AdicionarTentativa();
                    await eventoRepository.SaveChangesAsync();
                    continue;
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
    }
}