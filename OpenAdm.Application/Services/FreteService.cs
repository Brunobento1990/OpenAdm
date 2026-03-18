using OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracaoDeFreteModel;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class FreteService : IFreteService
{
    private readonly IHttpClientFrete _httpClientFrete;
    private readonly IConfiguracaoDeFreteRepository _configuracaoDeFreteRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IPesoRepository _pesoRepository;
    private readonly ITamanhoRepository _tamanhoRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;


    public FreteService(IHttpClientFrete httpClientFrete, IConfiguracaoDeFreteRepository configuracaoDeFreteRepository,
        IProdutoRepository produtoRepository, IPesoRepository pesoRepository, ITamanhoRepository tamanhoRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        _httpClientFrete = httpClientFrete;
        _configuracaoDeFreteRepository = configuracaoDeFreteRepository;
        _produtoRepository = produtoRepository;
        _pesoRepository = pesoRepository;
        _tamanhoRepository = tamanhoRepository;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<ResultPartner<CotacaoDeFreteViewModel>> CotarFreteAsync(CotacaoFreteDTO cotacaoFreteDto)
    {
        var erro = cotacaoFreteDto.Validar();

        if (!string.IsNullOrWhiteSpace(erro))
        {
            return (ResultPartner<CotacaoDeFreteViewModel>)erro;
        }

        var configuracaoDeFrete = await _configuracaoDeFreteRepository.ObterDoParceiroAsync(_parceiroAutenticado.Id);

        if (configuracaoDeFrete == null)
        {
            return (ResultPartner<CotacaoDeFreteViewModel>)
                "Não foi possível localizar as configurações de frete do parceiro, entre em contato com o adiministrador do ecommerce";
        }

        var produtos =
            await _produtoRepository.GetDictionaryProdutosAsync(cotacaoFreteDto.Produtos.Select(x => x.ProdutoId)
                .Distinct().ToList());

        var pesos = await _pesoRepository.GetDictionaryPesosByIdsAsync(cotacaoFreteDto.Produtos
            .Where(x => x.PesoId.HasValue).Select(x => x.PesoId!.Value).Distinct().ToList());

        var tamanhos = await _tamanhoRepository.GetDictionaryTamanhosAsync(cotacaoFreteDto.Produtos
            .Where(x => x.TamanhoId.HasValue).Select(x => x.TamanhoId!.Value).Distinct().ToList());

        var cepDestino = configuracaoDeFrete.CepOrigem;

        if (string.IsNullOrWhiteSpace(cepDestino))
        {
            var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();

            if (string.IsNullOrWhiteSpace(parceiro.EnderecoParceiro?.Cep))
            {
                return (ResultPartner<CotacaoDeFreteViewModel>)
                    "Não foi possível obter o CEP de origem do parceiro, entre em contato com o administrador do ecommerce";
            }

            cepDestino = parceiro.EnderecoParceiro.Cep;
        }

        var request = new CotacaoFreteRequest()
        {
            From = new()
            {
                Postal_code = cotacaoFreteDto.Cep
            },
            To = new()
            {
                Postal_code = cepDestino
            }
        };

        foreach (var item in cotacaoFreteDto.Produtos)
        {
            if (!produtos.TryGetValue(item.ProdutoId, out var produto))
            {
                return (ResultPartner<CotacaoDeFreteViewModel>)
                    "Não foi possível localizar um dos produtos selecionados";
            }

            var produtoRequest = new ProdutoCotacaoFreteRequest()
            {
                Id = produto.Id.ToString(),
                Quantity = (int)item.Quantidade
            };

            if (item.PesoId.HasValue && pesos.TryGetValue(item.PesoId.Value, out var peso))
            {
                produtoRequest.Height = peso.AlturaReal ?? 1;
                produtoRequest.Width = peso.LarguraReal ?? 1;
                produtoRequest.Weight = peso.PesoReal ?? 1;
                produtoRequest.Length = peso.ComprimentoReal ?? 1;
            }

            if (item.TamanhoId.HasValue && tamanhos.TryGetValue(item.TamanhoId.Value, out var tamanho))
            {
                produtoRequest.Height = tamanho.AlturaReal ?? 1;
                produtoRequest.Width = tamanho.LarguraReal ?? 1;
                produtoRequest.Weight = tamanho.PesoReal ?? 1;
                produtoRequest.Length = tamanho.ComprimentoReal ?? 1;
            }

            request.Products.Add(produtoRequest);
        }

        var resultado = await _httpClientFrete
            .CotarFreteAsync(request, configuracaoDeFrete.Token);

        if (resultado.Result == null)
        {
            return (ResultPartner<CotacaoDeFreteViewModel>)(resultado.Error ?? "Não foi possível cotar o frete");
        }

        return (ResultPartner<CotacaoDeFreteViewModel>)new CotacaoDeFreteViewModel()
        {
            Itens = resultado.Result.Dados.Where(x => x.FreteValido).Select(x => (ItemCotacaoDeFreteViewModel)x)
        };
    }
}