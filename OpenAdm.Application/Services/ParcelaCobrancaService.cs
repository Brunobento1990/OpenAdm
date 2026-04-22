using Microsoft.Extensions.Configuration;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.Model.Eventos;

namespace OpenAdm.Application.Services;

public class ParcelaCobrancaService : IParcelaCobrancaService
{
    private readonly IParcelaCobrancaRepository _parcelaCobrancaRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IHttpClientMercadoPago _httpClientMercadoPago;
    private readonly IConfiguration _configuration;
    private readonly IEventoAplicacaoRepository _eventoAplicacaoRepository;

    public ParcelaCobrancaService(IParcelaCobrancaRepository parcelaCobrancaRepository,
        IParceiroAutenticado parceiroAutenticado,
        IHttpClientMercadoPago httpClientMercadoPago,
        IConfiguration configuration,
        IEventoAplicacaoRepository eventoAplicacaoRepository)
    {
        _parcelaCobrancaRepository = parcelaCobrancaRepository;
        _parceiroAutenticado = parceiroAutenticado;
        _httpClientMercadoPago = httpClientMercadoPago;
        _configuration = configuration;
        _eventoAplicacaoRepository = eventoAplicacaoRepository;
    }

    public async Task<ResultPartner<ParcelaCobrancaViewModel>> ObterPorIdAsync(Guid id)
    {
        var parcelaCobranca = await _parcelaCobrancaRepository.ObterPorIdAsNoTrackingAsync(id);

        if (parcelaCobranca == null)
        {
            return (ResultPartner<ParcelaCobrancaViewModel>)"Não foi possível localizar a mensalidade";
        }

        var resultPix = await _httpClientMercadoPago
            .GerarPagamentoAsync(
                GerarBodyMercadoPado(parcelaCobranca, $"{_configuration["UrlWebHookCobranca"]}?parceiroId={_parceiroAutenticado.Id}"),
                _configuration["TokenMercadoPago"]!,
                parcelaCobranca.Id.ToString());

        var idExterno = resultPix.Id.ToString();

        if (!string.IsNullOrWhiteSpace(idExterno) && idExterno != parcelaCobranca.IdExterno)
        {
            await _parcelaCobrancaRepository.UpdateIdExternoAsync(parcelaCobranca.Id, idExterno);
        }

        var parcelaViewmodel = (ParcelaCobrancaViewModel)parcelaCobranca;

        parcelaViewmodel.Pix = new()
        {
            CopiaECola = resultPix.Point_of_interaction?.Transaction_data?.Qr_code ?? "",
            QrCode = resultPix.Point_of_interaction?.Transaction_data?.Qr_code_base64 ?? ""
        };

        return (ResultPartner<ParcelaCobrancaViewModel>)parcelaViewmodel;
    }

    public async Task<PaginacaoViewModel<ParcelaCobrancaViewModel>> PaginacaoAsync(FilterModel<ParcelaCobranca> filter)
    {
        filter.ParceiroId = _parceiroAutenticado.Id;

        var paginacao = await _parcelaCobrancaRepository.PaginacaoAsync(filter);

        return new PaginacaoViewModel<ParcelaCobrancaViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => (ParcelaCobrancaViewModel)x)
        };
    }

    private static MercadoPagoPagamentoRequest GerarBodyMercadoPado(ParcelaCobranca parcelaCobranca, string urlWebHook)
    {
        return new MercadoPagoPagamentoRequest()
        {
            Description = $"Mensalidade {parcelaCobranca.Numero}",
            Transaction_amount = parcelaCobranca.ValorPago,
            External_reference = parcelaCobranca.Id.ToString(),
            Notification_url = urlWebHook
        };
    }

    public async Task BaixarWebHookAsync(string idExterno, decimal valor)
    {
        var parcelaCobranca = await _parcelaCobrancaRepository.ObterPorIdExternoAsync(idExterno);

        if (parcelaCobranca == null || parcelaCobranca.Pago)
        {
            return;
        }

        parcelaCobranca.Pagar(valor);

        _parcelaCobrancaRepository.Update(parcelaCobranca);

        await _eventoAplicacaoRepository.AddAsync(EventoAplicacao.Criar(
            dados: new NotificarParceiroWhatsAppEvento()
            {
                Mensagem = $"💰 Pagamento recebido!\r\n\r\nSua mensalidade foi confirmada com sucesso.\r\n\r\n📅 Data: {DateTime.Now.DateTimeToString()}\r\n💵 Valor: R$ {valor.FormatMoney()}\r\n\r\nSe precisar de algo, é só responder essa mensagem 🙂"
            }.ToString(),
            tipoEventoAplicacao: Domain.Enuns.TipoEventoAplicacaoEnum.NotificarParceiroWhatsApp,
            empresaOpenAdmId: parcelaCobranca.EmpresaOpenAdmId));

        await _parcelaCobrancaRepository.SaveChangesAsync();
    }
}