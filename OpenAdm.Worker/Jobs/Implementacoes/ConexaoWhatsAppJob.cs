using OpenAdm.Domain.Extensions;
using OpenAdm.Worker.Application.DTOs;
using OpenAdm.Worker.Application.HttpService.Interface;
using OpenAdm.Worker.Application.Interfaces;
using OpenAdm.Worker.Application.Service;

namespace OpenAdm.Worker.Jobs.Implementacoes;

public class ConexaoWhatsAppJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly int _delayEmMinutos;
    private readonly string? _emailDeErro;

    public ConexaoWhatsAppJob(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _emailDeErro = configuration["Email:EmailDeErro"];
        _delayEmMinutos = int.TryParse(configuration["Jobs:ConexaoWhatsApp"], out var delay) ? delay : 30;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(_delayEmMinutos), stoppingToken);

            if (DateTime.Now.EhMadrugada())
            {
                LogService.Info("ConexaoWhatsApp: Madrugada");
                continue;
            }

            try
            {
                using var scoped = _serviceProvider.CreateScope();
                var servicoConexaoWhatsApp = scoped.ServiceProvider.GetService<IHttpClientWhatsApp>();

                if (servicoConexaoWhatsApp == null)
                {
                    LogService.Error("JOB serviço de verificação de conexão com whats app null");
                    continue;
                }

                var response = await servicoConexaoWhatsApp.StatusConexaoAsync();

                if (response.Result?.Instance?.Conectado == true)
                {
                    LogService.Success("Status conexão whats app OK");
                    continue;
                }

                if (string.IsNullOrEmpty(_emailDeErro))
                {
                    continue;
                }

                var servicoEmail = scoped.ServiceProvider.GetService<IEmailService>();

                if (servicoEmail == null)
                {
                    continue;
                }

                var emailModel = new EnviarEmailDTO()
                {
                    Assunto = "Erro de conexão whats app",
                    Email = _emailDeErro,
                    Mensagem = response.Error ?? "Não há erro específico de conexão",
                };

                await servicoEmail.EnviarAsync(emailModel);
            }
            catch (Exception e)
            {
                LogService.Error(e.InnerException?.Message ?? e.Message);
            }
        }
    }
}