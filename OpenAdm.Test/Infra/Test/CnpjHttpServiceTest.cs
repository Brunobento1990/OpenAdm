using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.Enums;
using OpenAdm.Infra.HttpService.Providers;
using OpenAdm.Infra.HttpService.Services;

namespace OpenAdm.Test.Infra.Test;

public class CnpjHttpServiceTest
{
    [Fact]
    public async Task DeveSelecionarProviderConfigurado()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ApiConsultaCnpj:Provider"] = "OpenCnpj"
            })
            .Build();
        var minhaReceita = new ProviderFake();
        var openCnpj = new ProviderFake();
        using var serviceProvider = CriarServiceProvider(minhaReceita, openCnpj);
        var service = new CnpjHttpService(configuration, serviceProvider, new CacheFake());

        await service.ConsultarCnpjAsync("68646809000129");

        Assert.False(minhaReceita.FoiConsultado);
        Assert.True(openCnpj.FoiConsultado);
    }

    [Fact]
    public async Task DeveUsarMinhaReceitaQuandoProviderNaoForConfigurado()
    {
        var configuration = new ConfigurationBuilder().Build();
        var minhaReceita = new ProviderFake();
        var openCnpj = new ProviderFake();
        using var serviceProvider = CriarServiceProvider(minhaReceita, openCnpj);
        var service = new CnpjHttpService(configuration, serviceProvider, new CacheFake());

        await service.ConsultarCnpjAsync("68646809000129");

        Assert.True(minhaReceita.FoiConsultado);
        Assert.False(openCnpj.FoiConsultado);
    }

    [Fact]
    public async Task DeveConsiderarProviderECnpjNaChaveDoCache()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ApiConsultaCnpj:Provider"] = "OpenCnpj"
            })
            .Build();
        var minhaReceita = new ProviderFake();
        var openCnpj = new ProviderFake();
        var cache = new CacheFake();
        using var serviceProvider = CriarServiceProvider(minhaReceita, openCnpj);
        var service = new CnpjHttpService(configuration, serviceProvider, cache);

        await service.ConsultarCnpjAsync("68646809000129");
        configuration["ApiConsultaCnpj:Provider"] = "MinhaReceita";
        await service.ConsultarCnpjAsync("68646809000129");
        await service.ConsultarCnpjAsync("68646809000129");

        Assert.Equal(1, openCnpj.QuantidadeConsultas);
        Assert.Equal(1, minhaReceita.QuantidadeConsultas);
        Assert.Contains("consulta_cnpj_OpenCnpj_68646809000129", cache.Itens.Keys);
        Assert.Contains("consulta_cnpj_MinhaReceita_68646809000129", cache.Itens.Keys);
    }

    private static ServiceProvider CriarServiceProvider(ProviderFake minhaReceita, ProviderFake openCnpj)
    {
        var services = new ServiceCollection();
        services.AddKeyedScoped<IConsultaCnpjProvider>(ConsultaCnpjProviderEnum.MinhaReceita,
            (_, _) => minhaReceita);
        services.AddKeyedScoped<IConsultaCnpjProvider>(ConsultaCnpjProviderEnum.OpenCnpj,
            (_, _) => openCnpj);
        return services.BuildServiceProvider();
    }

    private sealed class ProviderFake : IConsultaCnpjProvider
    {
        public bool FoiConsultado { get; private set; }
        public int QuantidadeConsultas { get; private set; }

        public Task<ConsultaCnpjResponse> ConsultarAsync(string cnpj)
        {
            FoiConsultado = true;
            QuantidadeConsultas++;
            return Task.FromResult(new ConsultaCnpjResponse { Cnpj = cnpj });
        }
    }

    private sealed class CacheFake : ICachedService<ConsultaCnpjResponse>
    {
        public Dictionary<string, ConsultaCnpjResponse> Itens { get; } = [];

        public Task<ConsultaCnpjResponse?> GetItemAsync(string key) =>
            Task.FromResult(Itens.GetValueOrDefault(key));

        public Task SetItemAsync(string key, ConsultaCnpjResponse item, TimeSpan? tempoExpiracao = null,
            TimeSpan? tempoExpiracaoDeslizante = null)
        {
            Itens[key] = item;
            return Task.CompletedTask;
        }

        public Task<IList<ConsultaCnpjResponse>?> GetListItemAsync(string key) =>
            Task.FromResult<IList<ConsultaCnpjResponse>?>(null);

        public Task SetListItemAsync(string key, IList<ConsultaCnpjResponse> itens) => Task.CompletedTask;

        public Task RemoveCachedAsync(string key) => Task.CompletedTask;
    }
}

public class OpenCnpjProviderTest
{
    private const string Json = """
        {
          "taxId": "68646809000129",
          "alias": "LJ Software",
          "founded": "2026-08-17",
          "head": true,
          "company": {
            "members": [{
              "since": "2026-08-17",
              "person": {
                "name": "Bruno Bento Goncalves",
                "taxId": "***996899**",
                "age": "31-40",
                "country": { "id": 76, "name": "Brasil" }
              },
              "role": { "id": 49, "text": "Sócio-Administrador" }
            }],
            "name": "LJ SOFTWARE LTDA",
            "equity": 5000,
            "nature": { "id": 2062, "text": "Sociedade Empresária Limitada" },
            "size": { "id": 1, "acronym": "ME", "text": "Microempresa" },
            "simples": { "optant": true, "since": "2026-08-18" },
            "simei": { "optant": false, "since": null }
          },
          "statusDate": "2026-08-17",
          "status": { "id": 2, "text": "Ativa" },
          "address": {
            "municipality": 4208203,
            "street": "Rua Luiz Castro",
            "number": "247",
            "district": "Santa Regina",
            "city": "Itajaí",
            "state": "SC",
            "details": "Casa 02",
            "zip": "88317521",
            "country": { "id": 76, "name": "Brasil" }
          },
          "phones": [{ "area": "81", "number": "83331687" }],
          "mainActivity": {
            "id": 6201501,
            "text": "Desenvolvimento de programas de computador sob encomenda"
          },
          "sideActivities": [{
            "id": 6203100,
            "text": "Desenvolvimento e licenciamento de programas de computador não customizáveis"
          }]
        }
        """;

    [Fact]
    public async Task DeveMapearRespostaParaContratoDaAplicacao()
    {
        var httpClient = new HttpClient(new JsonHttpMessageHandler(Json))
        {
            BaseAddress = new Uri("https://open.cnpja.com/office/")
        };
        var provider = new OpenCnpjProvider(new HttpClientFactoryFake(httpClient));

        var resultado = await provider.ConsultarAsync("68646809000129");

        Assert.Equal("68646809000129", resultado.Cnpj);
        Assert.Equal("LJ Software", resultado.NomeFantasia);
        Assert.Equal("LJ SOFTWARE LTDA", resultado.RazaoSocial);
        Assert.Equal(1, resultado.IdentificadorMatrizFilial);
        Assert.Equal(2, resultado.SituacaoCadastral);
        Assert.True(resultado.Ativa);
        Assert.Equal(6201501, resultado.CnaeFiscal);
        Assert.Equal("Itajaí", resultado.Municipio);
        Assert.Equal("8183331687", resultado.DddTelefone1);
        Assert.True(resultado.OpcaoPeloSimples);
        Assert.False(resultado.OpcaoPeloMei);
        Assert.Equal("Bruno Bento Goncalves", Assert.Single(resultado.Qsa).NomeSocio);
        Assert.Equal(6203100, Assert.Single(resultado.CnaesSecundarios).Codigo);
    }

    private sealed class HttpClientFactoryFake(HttpClient httpClient) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => httpClient;
    }

    private sealed class JsonHttpMessageHandler(string json) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });
    }
}
