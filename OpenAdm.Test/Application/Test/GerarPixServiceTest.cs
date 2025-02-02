using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;
using OpenAdm.Application.Services;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Repositories;
using OpenAdm.Test.Domain.Builder;
using OpenAdm.Test.Memory;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace OpenAdm.Test.Application.Test;

public class GerarPixServiceTest
{
    private readonly GerarPixPedidoService _gerarPixPedidoService;
    private readonly IFaturaRepository _faturaRepository;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly Mock<IHttpClientMercadoPago> _httpClientMercadoPago;
    private readonly Mock<IConfiguracaoDePagamentoService> _configuracaoDePagamentoService;
    private readonly IParcelaRepository _parcelaRepository;

    public GerarPixServiceTest()
    {
        var parceiroContext = ParceiroContextMemory.Init().Build();

        _faturaRepository = new FaturaRepository(parceiroContext);
        _pedidoRepository = new PedidoRepository(parceiroContext);
        _httpClientMercadoPago = new();
        _configuracaoDePagamentoService = new();
        _parcelaRepository = new ParcelaRepository(parceiroContext);

        _gerarPixPedidoService = new GerarPixPedidoService(
            faturaRepository: _faturaRepository,
            pedidoRepository: _pedidoRepository,
            httpClientMercadoPago: _httpClientMercadoPago.Object,
            configuracaoDePagamentoService: _configuracaoDePagamentoService.Object,
            parcelaRepository: _parcelaRepository);

    }

    [Fact]
    public async Task Deve_Gerar_Pix_Sem_Fatura_Com_Valor_Menor_Pedido()
    {
        var pedido = PedidoBuilder.Init().Build();
        await _pedidoRepository.AddAsync(pedido);

        var configuracaoPagamento = new ConfiguracaoDePagamentoViewModel();
        var mercadoPagoResponse = new MercadoPagoResponse()
        {
            Id = 5165,
            Point_of_interaction = new()
            {
                Transaction_data = new()
                {
                    Ticket_url = "url",
                    Qr_code_base64 = "qr code base64",
                    Qr_code = "qr code"
                }
            }
        };
        
        _configuracaoDePagamentoService.Setup(x => x.GetAsync()).ReturnsAsync(configuracaoPagamento);
        //_httpClientMercadoPago.Setup(x => x.PostAsync());

        var dto = new GerarPixParcelaDto()
        {
            PedidoId = pedido.Id,
            Valor = pedido.ValorTotal - 30
        };

        var result = await _gerarPixPedidoService.GerarPixAsync(dto);

        Assert.NotNull(result);
    }
}
