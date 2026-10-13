using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Infra.Enums;

namespace OpenAdm.Infra.HttpService.Providers;

public sealed class OpenCnpjProvider : ConsultaCnpjProviderBase, IConsultaCnpjProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OpenCnpjProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ConsultaCnpjResponse> ConsultarAsync(string cnpj)
    {
        using var client = _httpClientFactory.CreateClient(HttpServiceEnum.ConsultaCnpjOpenCnpj.ToString());
        using var response = await client.GetAsync(cnpj);
        var resultado = await ObterRespostaAsync<OpenCnpjResponse>(response);
        return Mapear(resultado);
    }

    private static ConsultaCnpjResponse Mapear(OpenCnpjResponse response)
    {
        var telefone1 = response.Phones?.ElementAtOrDefault(0);
        var telefone2 = response.Phones?.ElementAtOrDefault(1);

        return new ConsultaCnpjResponse
        {
            Cnpj = response.TaxId,
            IdentificadorMatrizFilial = response.Head ? 1 : 2,
            DescricaoIdentificadorMatrizFilial = response.Head ? "MATRIZ" : "FILIAL",
            NomeFantasia = response.Alias,
            SituacaoCadastral = response.Status?.Id ?? 0,
            DescricaoSituacaoCadastral = (response.Status?.Text ?? string.Empty).ToUpper(),
            Ativa = (response.Status?.Text ?? string.Empty).ToUpper() == "ATIVA",
            DataSituacaoCadastral = response.StatusDate,
            Pais = response.Address?.Country?.Name ?? string.Empty,
            CodigoPais = response.Address?.Country?.Id,
            DataInicioAtividade = response.Founded,
            CnaeFiscal = response.MainActivity?.Id ?? 0,
            CnaeFiscalDescricao = response.MainActivity?.Text ?? string.Empty,
            Logradouro = response.Address?.Street ?? string.Empty,
            Numero = response.Address?.Number ?? string.Empty,
            Complemento = response.Address?.Details ?? string.Empty,
            Bairro = response.Address?.District ?? string.Empty,
            Cep = response.Address?.Zip ?? string.Empty,
            Uf = response.Address?.State ?? string.Empty,
            CodigoMunicipioIbge = response.Address?.Municipality ?? 0,
            Municipio = response.Address?.City ?? string.Empty,
            DddTelefone1 = FormatarTelefone(telefone1),
            DddTelefone2 = FormatarTelefone(telefone2),
            OpcaoPeloSimples = response.Company?.Simples?.Optant,
            DataOpcaoPeloSimples = response.Company?.Simples?.Since,
            OpcaoPeloMei = response.Company?.Simei?.Optant,
            DataOpcaoPeloMei = response.Company?.Simei?.Since,
            RazaoSocial = response.Company?.Name ?? string.Empty,
            CodigoNaturezaJuridica = response.Company?.Nature?.Id ?? 0,
            NaturezaJuridica = response.Company?.Nature?.Text ?? string.Empty,
            CapitalSocial = response.Company?.Equity ?? 0,
            CodigoPorte = response.Company?.Size?.Id ?? 0,
            Porte = response.Company?.Size?.Text ?? string.Empty,
            DescricaoPorte = response.Company?.Size?.Acronym ?? string.Empty,
            Qsa = response.Company?.Members?.Select(MapearSocio).ToList() ?? [],
            CnaesSecundarios = response.SideActivities?.Select(x => new CnaeSecundario
            {
                Codigo = x.Id,
                Descricao = x.Text
            }).ToList() ?? []
        };
    }

    private static Qsa MapearSocio(OpenCnpjMember member) => new()
    {
        NomeSocio = member.Person?.Name ?? string.Empty,
        CnpjCpfDoSocio = member.Person?.TaxId ?? string.Empty,
        CodigoQualificacaoSocio = member.Role?.Id ?? 0,
        QualificacaoSocio = member.Role?.Text ?? string.Empty,
        DataEntradaSociedade = member.Since,
        CodigoPais = member.Person?.Country?.Id,
        Pais = member.Person?.Country?.Name ?? string.Empty,
        FaixaEtaria = member.Person?.Age ?? string.Empty
    };

    private static string FormatarTelefone(OpenCnpjPhone? telefone) =>
        telefone == null ? string.Empty : $"{telefone.Area}{telefone.Number}";
}

public sealed class OpenCnpjResponse
{
    public string TaxId { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public DateTime? Founded { get; set; }
    public bool Head { get; set; }
    public OpenCnpjCompany? Company { get; set; }
    public DateTime? StatusDate { get; set; }
    public OpenCnpjItem? Status { get; set; }
    public OpenCnpjAddress? Address { get; set; }
    public IList<OpenCnpjPhone>? Phones { get; set; }
    public OpenCnpjActivity? MainActivity { get; set; }
    public IList<OpenCnpjActivity>? SideActivities { get; set; }
}

public sealed class OpenCnpjCompany
{
    public IList<OpenCnpjMember>? Members { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Equity { get; set; }
    public OpenCnpjItem? Nature { get; set; }
    public OpenCnpjSize? Size { get; set; }
    public OpenCnpjTaxRegime? Simples { get; set; }
    public OpenCnpjTaxRegime? Simei { get; set; }
}

public sealed class OpenCnpjMember
{
    public DateTime? Since { get; set; }
    public OpenCnpjPerson? Person { get; set; }
    public OpenCnpjItem? Role { get; set; }
}

public sealed class OpenCnpjPerson
{
    public string Name { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Age { get; set; } = string.Empty;
    public OpenCnpjCountry? Country { get; set; }
}

public class OpenCnpjItem
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public sealed class OpenCnpjSize : OpenCnpjItem
{
    public string Acronym { get; set; } = string.Empty;
}

public sealed class OpenCnpjActivity
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public sealed class OpenCnpjTaxRegime
{
    public bool Optant { get; set; }
    public DateTime? Since { get; set; }
}

public sealed class OpenCnpjAddress
{
    public long Municipality { get; set; }
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public OpenCnpjCountry? Country { get; set; }
}

public sealed class OpenCnpjCountry
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class OpenCnpjPhone
{
    public string Area { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
}
