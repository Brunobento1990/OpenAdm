using System.Globalization;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Eventos;
using OpenAdm.Worker.Application.Service;

namespace OpenAdm.Worker.Jobs.Implementacoes;

public class GerarParcelaCobrancaJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public GerarParcelaCobrancaJob(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var agora = DateTime.Now;
            var proximaExecucao = agora.Date.AddDays(agora.Hour >= 7 ? 1 : 0)
                .AddHours(7);

            var delay = proximaExecucao - agora;

            await Task.Delay(delay, stoppingToken);

            if (DateTime.Now.EhFimDeSemana())
            {
                LogService.Info("GerarParcelaCobranca: Fim de semana");
                continue;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var parceiroRepository = scope.ServiceProvider.GetRequiredService<IParceiroRepository>();

                var parceirosParaCobranca = await parceiroRepository.ObterParaCobrancaAsync();

                if (parceirosParaCobranca.Count == 0)
                {
                    LogService.Info("Não há parceiro para cobrança");
                    continue;
                }

                var parcelaCobrancaRepository = scope.ServiceProvider.GetRequiredService<IParcelaCobrancaRepository>();
                var eventoAplicacaoRepository = scope.ServiceProvider.GetRequiredService<IEventoAplicacaoRepository>();

                foreach (var parceiro in parceirosParaCobranca)
                {
                    if (await parcelaCobrancaRepository.TemCobrancaAsync(parceiro.EmpresaOpenAdmId,
                            DateTime.UtcNow.Month,
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

                    var linkParaPagamento =
                        $"{parceiro.EmpresaOpenAdm.UrlAdmin}/financeiro/cobranca";

                    var mensagemCobranca =
                        $"Olá, {parceiro.NomeFantasia} 👋\nSua mensalidade já está disponível.\n📅 Competência: {DateTime.UtcNow.Month}/{DateTime.UtcNow.Year}\n💰 Valor: R$ {valor.FormatMoney()}\n📆 Vencimento: {parcelaCobranca.DataDeVencimento.DateTimeSomenteDataToString()}\n🔹 Link de pagamento:\n{linkParaPagamento}\nSe tiver qualquer dúvida, é só responder essa mensagem 🙂";

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
}