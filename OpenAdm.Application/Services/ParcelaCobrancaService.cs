using Microsoft.Extensions.Configuration;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Entities;
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
    private readonly IParceiroRepository _parceiroRepository;

    public ParcelaCobrancaService(IParcelaCobrancaRepository parcelaCobrancaRepository,
        IParceiroAutenticado parceiroAutenticado,
        IHttpClientMercadoPago httpClientMercadoPago,
        IConfiguration configuration,
        IEventoAplicacaoRepository eventoAplicacaoRepository, IParceiroRepository parceiroRepository)
    {
        _parcelaCobrancaRepository = parcelaCobrancaRepository;
        _parceiroAutenticado = parceiroAutenticado;
        _httpClientMercadoPago = httpClientMercadoPago;
        _configuration = configuration;
        _eventoAplicacaoRepository = eventoAplicacaoRepository;
        _parceiroRepository = parceiroRepository;
    }

    public async Task<ResultPartner<ParcelaCobrancaViewModel>> ObterPorIdAsync(Guid id)
    {
        var parcelaCobranca = await _parcelaCobrancaRepository.ObterPorIdAsNoTrackingAsync(id);

        if (parcelaCobranca == null)
        {
            return (ResultPartner<ParcelaCobrancaViewModel>)"Não foi possível localizar a mensalidade";
        }

        var parcelaViewmodel = (ParcelaCobrancaViewModel)parcelaCobranca;

        if (parcelaCobranca.Pago)
        {
            return (ResultPartner<ParcelaCobrancaViewModel>)parcelaViewmodel;
        }

        var parceiro = await _parceiroRepository.ObterPorEmpresaOpenAdmIdAsync(_parceiroAutenticado.Id);

        if (parceiro == null)
        {
            return (ResultPartner<ParcelaCobrancaViewModel>)"Não foi possível localizar seu cadastro";
        }

        var resultPix = await _httpClientMercadoPago
            .GerarPagamentoAsync(
                GerarBodyMercadoPado(parcelaCobranca,
                    $"{_configuration["UrlWebHookCobranca"]}?parceiroId={_parceiroAutenticado.Id}", parceiro),
                _configuration["TokenMercadoPago"]!,
                parcelaCobranca.Id.ToString());

        var idExterno = resultPix.Id.ToString();

        if (!string.IsNullOrWhiteSpace(idExterno) && idExterno != parcelaCobranca.IdExterno)
        {
            await _parcelaCobrancaRepository.UpdateIdExternoAsync(parcelaCobranca.Id, idExterno);
        }

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

    private static MercadoPagoPagamentoRequest GerarBodyMercadoPado(
        ParcelaCobranca parcelaCobranca,
        string urlWebHook,
        Parceiro parceiro)
    {
        return new MercadoPagoPagamentoRequest()
        {
            Description = $"Mensalidade {parcelaCobranca.Numero}",
            Transaction_amount = parcelaCobranca.Valor,
            External_reference = parcelaCobranca.Id.ToString(),
            Notification_url = urlWebHook,
            Payer = new()
            {
                Email = parceiro.Email ?? "",
                First_name = parceiro.NomeFantasia.Split(" ").FirstOrDefault() ?? "",
                Last_name = parceiro.NomeFantasia.Split(" ").LastOrDefault() ?? "",
                Identification = new()
                {
                    Type = parceiro.Cnpj.Length == 14 ? "CNPJ" : "CPF",
                    Number = parceiro.Cnpj
                }
            }
        };
    }

    public async Task BaixarWebHookAsync(string idExterno)
    {
        var parcelaCobranca = await _parcelaCobrancaRepository.ObterPorIdExternoAsync(idExterno);


        if (parcelaCobranca == null || parcelaCobranca.Pago)
        {
            return;
        }

        var pagamentoDoMercadoPago = await _httpClientMercadoPago.ObterPagamentoPagamentoAsync(idExterno,
            _configuration["TokenMercadoPago"]!);

        if (!pagamentoDoMercadoPago.Pago)
        {
            return;
        }

        parcelaCobranca.Pagar(parcelaCobranca.Valor);

        _parcelaCobrancaRepository.Update(parcelaCobranca);

        await _eventoAplicacaoRepository.AddAsync(EventoAplicacao.Criar(
            dados: new NotificarParceiroWhatsAppEvento()
            {
                Mensagem =
                    $"💰 Pagamento recebido!\r\n\r\nSua mensalidade foi confirmada com sucesso.\r\n\r\n📅 Data: {DateTime.Now.DateTimeToString()}\r\n💵 Valor: R$ {parcelaCobranca.Valor.FormatMoney()}\r\n\r\nSe precisar de algo, é só responder essa mensagem 🙂"
            }.ToString(),
            tipoEventoAplicacao: Domain.Enuns.TipoEventoAplicacaoEnum.NotificarParceiroWhatsApp,
            empresaOpenAdmId: parcelaCobranca.EmpresaOpenAdmId));

        await _parcelaCobrancaRepository.SaveChangesAsync();
    }
}