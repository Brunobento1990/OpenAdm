using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class GerarPixPedidoService : IGerarPixPedidoService
{
    private readonly IFaturaRepository _faturaRepository;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IHttpClientMercadoPago _httpClientMercadoPago;
    private readonly IConfiguracaoDePagamentoService _configuracaoDePagamentoService;
    private readonly IParcelaRepository _parcelaRepository;

    public GerarPixPedidoService(
        IFaturaRepository faturaRepository,
        IPedidoRepository pedidoRepository,
        IHttpClientMercadoPago httpClientMercadoPago,
        IConfiguracaoDePagamentoService configuracaoDePagamentoService,
        IParcelaRepository parcelaRepository)
    {
        _faturaRepository = faturaRepository;
        _pedidoRepository = pedidoRepository;
        _httpClientMercadoPago = httpClientMercadoPago;
        _configuracaoDePagamentoService = configuracaoDePagamentoService;
        _parcelaRepository = parcelaRepository;
    }

    public async Task<PagamentoViewModel> GerarPixAsync(GerarPixParcelaDto gerarPixParcelaDto)
    {
        gerarPixParcelaDto.Validar();
        var fatura = await _faturaRepository.GetByPedidoIdAsync(gerarPixParcelaDto.PedidoId);
        var configuracaoPagamento = await _configuracaoDePagamentoService.GetAsync()
                ?? throw new ExceptionApi("Não há configuração de pagamento para o mercado pago!", enviarErroDiscord: true);

        if (fatura == null || fatura.Pedido == null)
        {
            var pedido = await _pedidoRepository.GetPedidoByIdAsync(gerarPixParcelaDto.PedidoId)
                ?? throw new ExceptionApi("Nãi foi possível localizar o pedido");

            var mercadoPagoRequest = GerarBodyMercadoPado(pedido, gerarPixParcelaDto.Valor);
            var idempontencyKey = Guid.NewGuid();
            var resultPix = await _httpClientMercadoPago
                .PostAsync(mercadoPagoRequest, configuracaoPagamento.AccessToken, idempontencyKey.ToString());

            fatura = new Fatura(
                id: Guid.NewGuid(),
                dataDeCriacao: DateTime.Now,
                dataDeAtualizacao: DateTime.Now,
                numero: 0,
                status: StatusFaturaEnum.Aberta,
                usuarioId: pedido.UsuarioId,
                pedidoId: pedido.Id,
                dataDeFechamento: null,
                tipo: TipoFaturaEnum.A_Receber);

            fatura.Parcelas.Add(new Parcela(
                    id: idempontencyKey,
                    dataDeCriacao: DateTime.Now,
                    dataDeAtualizacao: DateTime.Now,
                    numero: 0,
                    dataDeVencimento: DateTime.Now,
                    numeroDaParcela: 1,
                    meioDePagamento: MeioDePagamentoEnum.Pix,
                    valor: gerarPixParcelaDto.Valor,
                    observacao: null,
                    faturaId: fatura.Id,
                    idExterno: resultPix.Id.ToString(),
                    desconto: null));

            if (gerarPixParcelaDto.Valor < pedido.ValorTotal)
            {
                fatura.Parcelas.Add(new Parcela(
                    id: Guid.NewGuid(),
                    dataDeCriacao: DateTime.Now,
                    dataDeAtualizacao: DateTime.Now,
                    numero: 0,
                    dataDeVencimento: DateTime.Now.AddMonths(1),
                    numeroDaParcela: 2,
                    meioDePagamento: MeioDePagamentoEnum.Pix,
                    valor: pedido.ValorTotal - gerarPixParcelaDto.Valor,
                    observacao: null,
                    faturaId: fatura.Id,
                    idExterno: null,
                    desconto: null));
            }

            await _faturaRepository.AddAsync(fatura);

            return new PagamentoViewModel()
            {
                LinkPagamento = resultPix.Point_of_interaction?.Transaction_data?.Ticket_url,
                QrCodePixBase64 = resultPix.Point_of_interaction?.Transaction_data?.Qr_code_base64,
                QrCodePix = resultPix.Point_of_interaction?.Transaction_data?.Qr_code
            };
        }

        if (fatura.ValorAPagarAReceber < gerarPixParcelaDto.Valor)
        {
            throw new ExceptionApi("O valor deve ser menor que o valor total do pedido");
        }

        var parcelasExcluir = fatura.Parcelas.Where(x => x.ValorPagoRecebido <= 0).ToList();

        if (parcelasExcluir.Count > 0)
        {
            _faturaRepository.ExcluirParcelasAsync(parcelasExcluir);
        }

        var mercadoPagoRequest1 = GerarBodyMercadoPado(fatura.Pedido, gerarPixParcelaDto.Valor);
        var idempontencyKey1 = Guid.NewGuid();
        var resultPix1 = await _httpClientMercadoPago
            .PostAsync(mercadoPagoRequest1, configuracaoPagamento.AccessToken, idempontencyKey1.ToString());

        if (gerarPixParcelaDto.Valor < fatura.ValorAPagarAReceber)
        {
            await _parcelaRepository.AddAsync(new Parcela(
                id: Guid.NewGuid(),
                dataDeCriacao: DateTime.Now,
                dataDeAtualizacao: DateTime.Now,
                numero: 0,
                dataDeVencimento: DateTime.Now.AddMonths(1),
                numeroDaParcela: 2,
                meioDePagamento: MeioDePagamentoEnum.Pix,
                valor: fatura.ValorAPagarAReceber - gerarPixParcelaDto.Valor,
                observacao: null,
                faturaId: fatura.Id,
                idExterno: null,
                desconto: null));
        }

        await _parcelaRepository.AddAsync(new Parcela(
                    id: idempontencyKey1,
                    dataDeCriacao: DateTime.Now,
                    dataDeAtualizacao: DateTime.Now,
                    numero: 0,
                    dataDeVencimento: DateTime.Now,
                    numeroDaParcela: 1,
                    meioDePagamento: MeioDePagamentoEnum.Pix,
                    valor: gerarPixParcelaDto.Valor,
                    observacao: null,
                    faturaId: fatura.Id,
                    idExterno: resultPix1.Id.ToString(),
                    desconto: null));

        return new PagamentoViewModel()
        {
            LinkPagamento = resultPix1.Point_of_interaction?.Transaction_data?.Ticket_url,
            QrCodePixBase64 = resultPix1.Point_of_interaction?.Transaction_data?.Qr_code_base64,
            QrCodePix = resultPix1.Point_of_interaction?.Transaction_data?.Qr_code
        };
    }

    private static MercadoPagoRequest GerarBodyMercadoPado(Pedido pedido, decimal valor)
    {
        return new MercadoPagoRequest()
        {
            Description = $"Pedido {pedido.Numero}",
            Transaction_amount = valor,
            External_reference = pedido.Id.ToString(),
            Notification_url = $"https://api.hml.iscaslune.com.br/api/pagamento/notificar",
            Payer = new()
            {
                Email = pedido.Usuario.Email,
                First_name = pedido.Usuario.Nome,
                Identification = new()
                {
                    Type = string.IsNullOrWhiteSpace(pedido.Usuario.Cnpj) ? "CPF" : "CNPJ",
                    Number = string.IsNullOrWhiteSpace(pedido.Usuario.Cnpj) ? pedido.Usuario.Cpf ?? "" : pedido.Usuario.Cnpj ?? ""
                }
            }
        };
    }
}
