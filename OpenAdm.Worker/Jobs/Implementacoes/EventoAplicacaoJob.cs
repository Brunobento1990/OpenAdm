using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Eventos;
using OpenAdm.Worker.Application.Interfaces;
using OpenAdm.Worker.Application.Service;

namespace OpenAdm.Worker.Jobs.Implementacoes;

public class EventoAplicacaoJob : BackgroundService
{
    private readonly IFilaService _fila;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public EventoAplicacaoJob(IFilaService fila, IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _fila = fila;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = await _fila.InscreverAsync(EventoBase.FilaEventoAplicacao);

        while (!stoppingToken.IsCancellationRequested)
        {
            var mensagem = await consumer.LerAsync(stoppingToken);

            if (mensagem is null)
                continue;

            if (_configuration.GetValue<bool>("Jobs:EventoAplicacaoInativo"))
            {
                LogService.Info("Evento Aplicacao Inativo");
                continue;
            }

            await using var scope = _serviceProvider.CreateAsyncScope();

            var evento = EventoAplicacao.Desserealizar(mensagem.Conteudo);

            if (evento == null || !evento.PodeExecutar)
            {
                continue;
            }

            var parceiroRepository = scope.ServiceProvider.GetRequiredService<IEmpresaOpenAdmRepository>();
            var parceiroAutenticado = scope.ServiceProvider.GetRequiredService<IParceiroAutenticado>();

            var parceiro = await parceiroRepository.ObterPorIdAsync(evento.EmpresaOpenAdmId);

            if (parceiro == null)
            {
                await consumer.ConfirmarAsync(mensagem.Id);
                continue;
            }
            
            parceiroAutenticado.ConnectionString = Criptografia.Decrypt(parceiro.ConnectionString);
            parceiroAutenticado.Id = evento.EmpresaOpenAdmId;

            try
            {
                var eventoServico =
                    scope.ServiceProvider.GetKeyedService<IEventoAplicacaoService>(evento.TipoEventoAplicacao);

                if (eventoServico == null)
                {
                    LogService.Info($"Evento sem implementação: {evento.TipoEventoAplicacao}");
                    continue;
                }

                await eventoServico.ExecutarAsync(evento);
            }
            catch (Exception ex)
            {
                LogService.Error($"Erro no evento: {ex.InnerException?.Message ?? ex.Message}");
            }
            finally
            {
                await consumer.ConfirmarAsync(mensagem.Id);
            }
        }
    }
}