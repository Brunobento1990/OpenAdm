using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pagamentos;
using OpenAdm.Application.Models.CobrancasPedidosEcommerce;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class CobrancaPedidoService : ICobrancaPedidoService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguracaoDePagamentoService _configuracaoDePagamentoService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguration _configuration;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IFaturaRepository _faturaRepository;
    private readonly ICobrancaPedidoEcommerceRepository _cobrancaPedidoRepository;

    public CobrancaPedidoService(IServiceProvider serviceProvider,
        IConfiguracaoDePagamentoService configuracaoDePagamentoService, IPedidoRepository pedidoRepository,
        IConfiguration configuration, IParceiroAutenticado parceiroAutenticado, IFaturaRepository faturaRepository,
        ICobrancaPedidoEcommerceRepository cobrancaPedidoRepository)
    {
        _serviceProvider = serviceProvider;
        _configuracaoDePagamentoService = configuracaoDePagamentoService;
        _pedidoRepository = pedidoRepository;
        _configuration = configuration;
        _parceiroAutenticado = parceiroAutenticado;
        _faturaRepository = faturaRepository;
        _cobrancaPedidoRepository = cobrancaPedidoRepository;
    }

    public async Task<ResultPartner<CobrancaPedidoViewModel>> GetParaNegociacaoAsync(Guid pedidoId)
    {
        var cobranca = await _cobrancaPedidoRepository.GetByPedidoIdAsync(pedidoId, _parceiroAutenticado.Id);

        if (cobranca == null)
        {
            return (ResultPartner<CobrancaPedidoViewModel>)"Não foi possível localizar a cobrança do pedido";
        }

        if (cobranca.Status != StatusCobrancaPedidoEcommerceEnum.ACobrar)
        {
            return (ResultPartner<CobrancaPedidoViewModel>)"A cobrança do pedido já foi negociada";
        }

        return (ResultPartner<CobrancaPedidoViewModel>)(CobrancaPedidoViewModel)cobranca;
    }

    public async Task<ResultPartner<PagamentoViewModel>> CobrarAsync(GerarCobrancaPedidoDto gerarCobrancaPedidoDto)
    {
        var service =
            _serviceProvider.GetKeyedService<IGerarCobrancaPedidoService>(gerarCobrancaPedidoDto.MeioDePagamento);

        if (service == null)
        {
            return (ResultPartner<PagamentoViewModel>)"Não há implementação para o meio de pagamento selecionado";
        }

        var configuracao = await _configuracaoDePagamentoService.GetAsync();

        if (string.IsNullOrWhiteSpace(configuracao?.AccessToken))
        {
            return (ResultPartner<PagamentoViewModel>)"Esse ecommecer não tem uma configuração de pagamento ativa";
        }

        var pedido = await _pedidoRepository.ObterPedidoParaCobrancaAsync(gerarCobrancaPedidoDto.PedidoId);

        if (pedido == null)
        {
            return (ResultPartner<PagamentoViewModel>)"Não foi possível localizar o pedido";
        }

        var erroDeCobranca = pedido.PodeCobrar();

        if (!string.IsNullOrWhiteSpace(erroDeCobranca))
        {
            return (ResultPartner<PagamentoViewModel>)erroDeCobranca;
        }

        var resultado = await service.GerarCobrancaAsync(pedido,
            $"{_configuration["UrlWebHook"]}?parceiroId={_parceiroAutenticado.Id}", configuracao.AccessToken);

        if (resultado.Result == null)
        {
            return resultado;
        }

        if (pedido.Fatura == null)
        {
            //TODO: verificar porque tinha o método de nova fatura aqui
            // var fatura = Fatura
            //     .NovaContasAReceber(
            //         usuarioId: pedido.UsuarioId,
            //         total: pedido.ValorTotalCobrar,
            //         pedidoId: pedido.Id,
            //         quantidadeDeParcelas: 1,
            //         primeiroVencimento: DateTime.Now.AddDays(1),
            //         meioDePagamento: gerarCobrancaPedidoDto.MeioDePagamento,
            //         desconto: null,
            //         observacao: null,
            //         idExterno: resultado.Result.IdExterno,
            //         tipo: TipoFaturaEnum.A_Receber);

            //await _faturaRepository.AdicionarAsync(fatura);
            //await _faturaRepository.SaveChangesAsync();
        }

        return resultado;
    }
}
