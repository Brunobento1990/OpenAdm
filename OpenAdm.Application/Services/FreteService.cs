using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.Fretes;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Fretes;
using OpenAdm.Domain.Interfaces;
using System.Text.Json;

namespace OpenAdm.Application.Services;

public class FreteService : IFreteService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracaoDeFreteRepository _configuracaoDeFreteRepository;
    public FreteService(
        IHttpClientFactory httpClientFactory,
        IPedidoRepository pedidoRepository,
        IConfiguracaoDeFreteRepository configuracaoDeFreteRepository)
    {
        _httpClientFactory = httpClientFactory;
        _pedidoRepository = pedidoRepository;
        _configuracaoDeFreteRepository = configuracaoDeFreteRepository;
    }

    public async Task<FreteViewModel> CalcularAsync(CalcularFretePedidoDto calcularFretePedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(calcularFretePedidoDto.PedidoId)
            ?? throw new ExceptionApi("Não foi possível localizar o pedido!");

        var configuracao = await _configuracaoDeFreteRepository.GetConfiguracaoAsync()
            ?? throw new Exception("Não foi localizada a configuração de frete!");

        var peso = (int)pedido.ItensPedido.Sum(x => x.Produto.Peso ?? (decimal)0.1);
        var calcularFreteDto = new CalcularFreteDto() 
        {
            CepDestino = calcularFretePedidoDto.Cep,
            TipoFrete = calcularFretePedidoDto.TipoFrete,
            Altura = configuracao.AlturaEmbalagem,
            CepOrigem = configuracao.CepOrigem,
            Comprimento = configuracao.ComprimentoEmbalagem,
            Largura = configuracao.LarguraEmbalagem,
            Peso = peso > 0 ? peso : 1,
        };

        var client = _httpClientFactory.CreateClient("Frete");
        var response = await client.PostAsync("frete/calcular", calcularFreteDto.ToJson());

        if (!response.IsSuccessStatusCode)
        {
            var erroJson = await response.Content.ReadAsStringAsync();

            var erro = JsonSerializer.Deserialize<ErrorResponse>(erroJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })
                ?? throw new Exception("Não foi possível calcular o frete!");

            throw new ExceptionApi(erro.Mensagem);
        }

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FreteViewModel>(result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true})
            ?? throw new Exception("Não foi possível calcular o frete!");
    }
}
