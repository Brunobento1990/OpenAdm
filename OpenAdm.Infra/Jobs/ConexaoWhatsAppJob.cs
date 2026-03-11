using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Emails;
using Serilog;

namespace OpenAdm.Infra.Jobs;

public class ConexaoWhatsAppJob : BaseJob, IJobInfo
{
    private readonly IServiceProvider _serviceProvider;

    public ConexaoWhatsAppJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
            var servicoConexaoWhatsApp = scoped.ServiceProvider.GetService<IWhatsAppHttpClient>();

            if (servicoConexaoWhatsApp == null)
            {
                Log.Warning("JOB serviço de verificação de conexão com whats app null");
                return;
            }

            var response = await servicoConexaoWhatsApp.StatusConexaoAsync();

            if (response.Result?.Instance?.Conectado == true)
            {
                return;
            }

            var servicoEmail = scoped.ServiceProvider.GetService<IEmailApiService>();

            if (servicoEmail == null)
            {
                return;
            }

            var fromEnvioEmail = new FromEnvioEmailApiModel()
            {
                Email = EmailConfiguracaoModel.Email,
                EnableSsl = true,
                Porta = EmailConfiguracaoModel.Porta,
                Senha = EmailConfiguracaoModel.Senha,
                Servidor = EmailConfiguracaoModel.Servidor
            };

            var emailModel = new ToEnvioEmailApiModel()
            {
                Assunto = "Erro de conexão whats app",
                Email = EmailConfiguracaoModel.Email,
                Mensagem = response.Error ?? "Não há erro específico de conexão",
            };

            await servicoEmail.SendEmailAsync(emailModel, fromEnvioEmail);
        }
        catch (Exception e)
        {
            Log.Error(e, "Erro Job ConexaoWhatsApp");
        }
    }
}