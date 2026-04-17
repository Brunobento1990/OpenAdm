using OpenAdm.Worker.Application.DTOs;
using OpenAdm.Worker.Application.HttpService.Interface;
using OpenAdm.Worker.Application.Interfaces;
using OpenAdm.Worker.Application.Service;
using OpenAdm.Worker.Jobs.Interfaces;

namespace OpenAdm.Worker.Jobs.Implementacoes;

public class ConexaoWhatsAppJob : BaseJob, IJobInfo
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public ConexaoWhatsAppJob(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public static string Key => "ConexaoWhatsAppJobKey";
    public static string IdentityTrigger => "ConexaoWhatsAppJobTrigger";

    public static string ObterConfiguracaoTempoDeExecucao(IConfiguration configuracao)
    {
        var cron = configuracao["Jobs:ConexaoWhatsApp"];
        return string.IsNullOrWhiteSpace(cron) ? "0 */1 * * * ?" : cron;
    }

    public override async Task ExecuteTask()
    {
        try
        {
            using var scoped = _serviceProvider.CreateScope();
            var servicoConexaoWhatsApp = scoped.ServiceProvider.GetService<IHttpClientWhatsApp>();

            if (servicoConexaoWhatsApp == null)
            {
                LogService.Error("JOB serviço de verificação de conexão com whats app null");
                return;
            }

            var response = await servicoConexaoWhatsApp.StatusConexaoAsync();

            if (response.Result?.Instance?.Conectado == true)
            {
                LogService.Success("Status conexão whats app OK");
                return;
            }

            var servicoEmail = scoped.ServiceProvider.GetService<IEmailService>();

            if (servicoEmail == null)
            {
                return;
            }

            var emailModel = new EnviarEmailDTO()
            {
                Assunto = "Erro de conexão whats app",
                Email = _configuration["Email:EmailDeErro"]!,
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