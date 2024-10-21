using OpenAdm.Application.Dtos.Ceps;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Fretes;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.Model;

namespace OpenAdm.Application.Services;

public sealed class FreteService : IFreteService
{
    private readonly ICepHttpService _cepHttpService;
    private readonly IConfiguracaoDeFreteService _configuracaoDeFreteService;
    private readonly IItensPedidoRepository _itensPedidoRepository;
    public FreteService(
        ICepHttpService cepHttpService,
        IConfiguracaoDeFreteService configuracaoDeFreteService,
        IItensPedidoRepository itensPedidoRepository)
    {
        _cepHttpService = cepHttpService;
        _configuracaoDeFreteService = configuracaoDeFreteService;
        _itensPedidoRepository = itensPedidoRepository;
    }

    public async Task<FreteViewModel> CotarFreteAsync(CotarFreteDto cotarFreteDto)
    {
        cotarFreteDto.Validar();
        var configuracaoFrete = await _configuracaoDeFreteService.GetAsync()
            ?? throw new ExceptionApi("Não foi possível localizar a configuração de frete, tente novamente!", enviarErroDiscord: true);
        var itensPedido = await _itensPedidoRepository.GetItensPedidoByPedidoIdAsync(pedidoId: cotarFreteDto.PedidoId);

        if (itensPedido == null || itensPedido.Count == 0)
        {
            throw new ExceptionApi("Não foi possível localizar os itens do seu pedido, tente novamente!", enviarErroDiscord: true);
        }

        var consultaCepResponse = await _cepHttpService.ConsultaCepAsync(cotarFreteDto.CepDestino);
        decimal totalPeso = itensPedido.Where(x => x.Peso != null).Sum(x => x.Peso!.PesoReal ?? 1);
        totalPeso += itensPedido.Where(x => x.Tamanho != null).Sum(x => x.Tamanho!.PesoReal ?? 1);
        totalPeso += configuracaoFrete.Peso ?? 1;

        var cepRequest = new CotacaoFreteRequest()
        {
            Peso = ((int)totalPeso).ToString(),
            CepDestino = cotarFreteDto.CepDestino,
            Altura = configuracaoFrete.AlturaEmbalagem,
            CepOrigem = configuracaoFrete.CepOrigem,
            ChaveDeAcesso = configuracaoFrete.ChaveApi,
            Comprimento = configuracaoFrete.ComprimentoEmbalagem,
            Largura = configuracaoFrete.LarguraEmbalagem
        };

        var cotacaoFrete = await _cepHttpService.CotarFreteAsync(cepRequest);
        cotacaoFrete.Validar();

        return new()
        {
            ValorPac = decimal.Parse(cotacaoFrete.Valorpac.Replace(",", ".")),
            ValorSedex = decimal.Parse(cotacaoFrete.Valorsedex.Replace(",", ".")),
            Endereco = new()
            {
                Bairro = consultaCepResponse.Bairro,
                Complemento = consultaCepResponse.Complemento ?? string.Empty,
                Localidade = consultaCepResponse.Localidade,
                Logradouro = consultaCepResponse.Logradouro,
                Uf = consultaCepResponse.Uf
            }
        };
    }
}
