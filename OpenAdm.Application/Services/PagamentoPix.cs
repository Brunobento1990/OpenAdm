using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.Model;

namespace OpenAdm.Application.Services;

public sealed class PagamentoPix : IPagamentoService
{
    private readonly IPedidoRepository _pedidoService;
    private readonly IEnderecoEntregaPedidoService _enderecoEntregaPedidoService;
    private readonly IConfiguracaoParceiroRepository _configuracaoParceiroRepository;
    private readonly IMercadoPagoHttpService _mercadoPagoHttpService;
    private readonly IConfiguracaoDePagamentoService _configuracaoDePagamentoService;
    public PagamentoPix(
        IPedidoRepository pedidoService,
        IEnderecoEntregaPedidoService enderecoEntregaPedidoService,
        IConfiguracaoParceiroRepository configuracaoParceiroRepository,
        IMercadoPagoHttpService mercadoPagoHttpService,
        IConfiguracaoDePagamentoService configuracaoDePagamentoService)
    {
        _pedidoService = pedidoService;
        _enderecoEntregaPedidoService = enderecoEntregaPedidoService;
        _configuracaoParceiroRepository = configuracaoParceiroRepository;
        _mercadoPagoHttpService = mercadoPagoHttpService;
        _configuracaoDePagamentoService = configuracaoDePagamentoService;
    }

    public async Task<PagamentoViewModel> GerarPagamentoAsync(Guid pedidoId)
    {
        var pedido = await _pedidoService.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new ExceptionApi("Não foi possível localizar seu pedido para gerar o pix!");

        var enderecoEntrega = await _enderecoEntregaPedidoService.GetByPedidoIdAsync(pedidoId);
        var configuracao = await _configuracaoParceiroRepository.GetParceiroAutenticadoAdmAsync();

        var mercadoPagoRequest = new MercadoPagoRequest()
        {
            Description = $"Pedido {pedido.Numero}",
            Transaction_amount = pedido.ValorTotal + (enderecoEntrega?.ValorFrete ?? 0),
            External_reference = pedido.Id.ToString(),
            Notification_url = $"https://api.open-adm.tech/api/v1/pagamento/pagamento/notificar?cliente={configuracao?.ClienteMercadoPago ?? ""}",
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

        var configuracaoPagamento = await _configuracaoDePagamentoService.GetAsync()
            ?? throw new ExceptionApi("Não há configuração de pagamento!", enviarErroDiscord: true);

        var result = await _mercadoPagoHttpService.PostAsync(
            mercadoPagoRequest,
            configuracaoPagamento.AccessToken,
            pedido.Id.ToString());

        return new PagamentoViewModel()
        {
            LinkPagamento = result.Point_of_interaction?.Transaction_data?.Ticket_url,
            QrCodePixBase64 = result.Point_of_interaction?.Transaction_data?.Qr_code_base64,
            QrCodePix = result.Point_of_interaction?.Transaction_data?.Qr_code,
            MercadoPagoId = result.Id.ToString(),
            Total = pedido.ValorTotal,
            PrimeiroVencimento = DateTime.Now.AddDays(1),
            QuantidadeDeParcelas = 1
        };
    }
}
