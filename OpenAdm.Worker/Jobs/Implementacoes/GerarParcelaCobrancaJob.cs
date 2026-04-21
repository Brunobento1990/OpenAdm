using System.Globalization;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Eventos;
using OpenAdm.Worker.Application.Service;
using OpenAdm.Worker.Jobs.Interfaces;

namespace OpenAdm.Worker.Jobs.Implementacoes;

public class GerarParcelaCobrancaJob : BaseJob, IJobInfo
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public GerarParcelaCobrancaJob(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public static string Key => "GerarParcelaCobrancaJobKey";
    public static string IdentityTrigger => "GerarParcelaCobrancaJobTrigger";

    public static string ObterConfiguracaoTempoDeExecucao(IConfiguration configuracao)
    {
        var cron = configuracao["Jobs:GerarParcelaCobranca"];
        return string.IsNullOrWhiteSpace(cron) ? "0 */1 * * * ?" : cron;
    }

    public override async Task ExecuteTask()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var parceiroRepository = scope.ServiceProvider.GetRequiredService<IParceiroRepository>();

            var parceirosParaCobranca = await parceiroRepository.ObterParaCobrancaAsync();

            if (parceirosParaCobranca.Count == 0)
            {
                LogService.Info("Não há parceiro para cobrança");
                return;
            }

            var parcelaCobrancaRepository = scope.ServiceProvider.GetRequiredService<IParcelaCobrancaRepository>();
            var eventoAplicacaoRepository = scope.ServiceProvider.GetRequiredService<IEventoAplicacaoRepository>();

            foreach (var parceiro in parceirosParaCobranca)
            {
                if (await parcelaCobrancaRepository.TemCobrancaAsync(parceiro.EmpresaOpenAdmId, DateTime.UtcNow.Month,
                        DateTime.UtcNow.Year))
                {
                    LogService.Info($"Já tem mensalidado para o parceiro: {parceiro.NomeFantasia}");
                    continue;
                }

                var proximoNumero = await parcelaCobrancaRepository.ProximoNumeroParcela(parceiro.EmpresaOpenAdmId);
                var valor = decimal.Parse(_configuration["ParcelaCobranca:Valor"]!, CultureInfo.InvariantCulture);

                var parcelaCobranca = ParcelaCobranca.NovaCobranca(
                    empresaOpenAdmId: parceiro.EmpresaOpenAdmId,
                    numero: proximoNumero,
                    mesCobranca: DateTime.UtcNow.Month,
                    anoCobranca: DateTime.UtcNow.Year,
                    tipoParcelaCobranca: parceiro.EmpresaOpenAdm.TipoParcelaCobranca,
                    valor: valor,
                    dataDeVencimento: DateTime.UtcNow.AddDays(
                        int.Parse(_configuration["ParcelaCobranca:DiasVencimento"]!))
                );

                var mensagemCobranca =
                    $"Olá, {parceiro.NomeFantasia} 👋\nSua mensalidade já está disponível.\n📅 Competência: {DateTime.UtcNow.Month}/{DateTime.UtcNow.Year}\n💰 Valor: R$ {valor.FormatMoney()}\n📆 Vencimento: {parcelaCobranca.DataDeVencimento.DateTimeSomenteDataToString()}\nPara realizar o pagamento, escolha uma das opções abaixo:\n🔹 PIX (copia e cola):\nPIX_COPIA_E_COLA\n🔹 Link de pagamento:\nLINK_PAGAMENTO\nSe tiver qualquer dúvida, é só responder essa mensagem 🙂";

                var dados = new NotificarParceiroWhatsAppEvento()
                {
                    Mensagem = mensagemCobranca,
                }.ToString();

                await eventoAplicacaoRepository.AddAsync(EventoAplicacao.Criar(
                    dados: dados,
                    tipoEventoAplicacao: TipoEventoAplicacaoEnum.NotificarParceiroWhatsApp,
                    empresaOpenAdmId: parceiro.EmpresaOpenAdmId));
                await parcelaCobrancaRepository.AddAsync(parcelaCobranca);

                await parcelaCobrancaRepository.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            LogService.Error(e.InnerException?.Message ?? e.Message);
        }
    }
}