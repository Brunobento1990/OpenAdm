using System.Text.Json.Serialization;

namespace OpenAdm.Application.HttpClient.Response;

public class ConsultaCnpjResponse
{
    [JsonPropertyName("cnpj")]
    public string Cnpj { get; set; } = string.Empty;

    [JsonPropertyName("identificador_matriz_filial")]
    public int IdentificadorMatrizFilial { get; set; }

    [JsonPropertyName("descricao_identificador_matriz_filial")]
    public string DescricaoIdentificadorMatrizFilial { get; set; } = string.Empty;

    [JsonPropertyName("nome_fantasia")]
    public string NomeFantasia { get; set; } = string.Empty;

    [JsonPropertyName("situacao_cadastral")]
    public int SituacaoCadastral { get; set; }

    [JsonPropertyName("descricao_situacao_cadastral")]
    public string DescricaoSituacaoCadastral { get; set; } = string.Empty;

    [JsonPropertyName("data_situacao_cadastral")]
    public DateTime? DataSituacaoCadastral { get; set; }

    [JsonPropertyName("motivo_situacao_cadastral")]
    public int MotivoSituacaoCadastral { get; set; }

    [JsonPropertyName("descricao_motivo_situacao_cadastral")]
    public string DescricaoMotivoSituacaoCadastral { get; set; } = string.Empty;

    [JsonPropertyName("nome_cidade_no_exterior")]
    public string NomeCidadeNoExterior { get; set; } = string.Empty;

    [JsonPropertyName("codigo_pais")]
    public int? CodigoPais { get; set; }

    [JsonPropertyName("pais")]
    public string Pais { get; set; } = string.Empty;

    [JsonPropertyName("data_inicio_atividade")]
    public DateTime? DataInicioAtividade { get; set; }

    [JsonPropertyName("cnae_fiscal")]
    public long CnaeFiscal { get; set; }

    [JsonPropertyName("cnae_fiscal_descricao")]
    public string CnaeFiscalDescricao { get; set; } = string.Empty;

    [JsonPropertyName("descricao_tipo_de_logradouro")]
    public string DescricaoTipoDeLogradouro { get; set; } = string.Empty;

    [JsonPropertyName("logradouro")]
    public string Logradouro { get; set; } = string.Empty;

    [JsonPropertyName("numero")]
    public string Numero { get; set; } = string.Empty;

    [JsonPropertyName("complemento")]
    public string Complemento { get; set; } = string.Empty;

    [JsonPropertyName("bairro")]
    public string Bairro { get; set; } = string.Empty;

    [JsonPropertyName("cep")]
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("uf")]
    public string Uf { get; set; } = string.Empty;

    [JsonPropertyName("codigo_municipio")]
    public int CodigoMunicipio { get; set; }

    [JsonPropertyName("codigo_municipio_ibge")]
    public long CodigoMunicipioIbge { get; set; }

    [JsonPropertyName("municipio")]
    public string Municipio { get; set; } = string.Empty;

    [JsonPropertyName("ddd_telefone_1")]
    public string DddTelefone1 { get; set; } = string.Empty;

    [JsonPropertyName("ddd_telefone_2")]
    public string DddTelefone2 { get; set; } = string.Empty;

    [JsonPropertyName("ddd_fax")]
    public string DddFax { get; set; } = string.Empty;

    [JsonPropertyName("situacao_especial")]
    public string SituacaoEspecial { get; set; } = string.Empty;

    [JsonPropertyName("data_situacao_especial")]
    public DateTime? DataSituacaoEspecial { get; set; }

    [JsonPropertyName("opcao_pelo_simples")]
    public bool? OpcaoPeloSimples { get; set; }

    [JsonPropertyName("data_opcao_pelo_simples")]
    public DateTime? DataOpcaoPeloSimples { get; set; }

    [JsonPropertyName("data_exclusao_do_simples")]
    public DateTime? DataExclusaoDoSimples { get; set; }

    [JsonPropertyName("opcao_pelo_mei")]
    public bool? OpcaoPeloMei { get; set; }

    [JsonPropertyName("data_opcao_pelo_mei")]
    public DateTime? DataOpcaoPeloMei { get; set; }

    [JsonPropertyName("data_exclusao_do_mei")]
    public DateTime? DataExclusaoDoMei { get; set; }

    [JsonPropertyName("razao_social")]
    public string RazaoSocial { get; set; } = string.Empty;

    [JsonPropertyName("codigo_natureza_juridica")]
    public int CodigoNaturezaJuridica { get; set; }

    [JsonPropertyName("natureza_juridica")]
    public string NaturezaJuridica { get; set; } = string.Empty;

    [JsonPropertyName("qualificacao_do_responsavel")]
    public int QualificacaoDoResponsavel { get; set; }

    [JsonPropertyName("capital_social")]
    public decimal CapitalSocial { get; set; }

    [JsonPropertyName("codigo_porte")]
    public int CodigoPorte { get; set; }

    [JsonPropertyName("porte")]
    public string Porte { get; set; } = string.Empty;

    [JsonPropertyName("ente_federativo_responsavel")]
    public string EnteFederativoResponsavel { get; set; } = string.Empty;

    [JsonPropertyName("descricao_porte")]
    public string DescricaoPorte { get; set; } = string.Empty;

    [JsonPropertyName("qsa")]
    public IList<Qsa> Qsa { get; set; } = [];

    [JsonPropertyName("cnaes_secundarios")]
    public IList<CnaeSecundario> CnaesSecundarios { get; set; } = [];
}

public class Qsa
{
    [JsonPropertyName("identificador_de_socio")]
    public int IdentificadorDeSocio { get; set; }

    [JsonPropertyName("nome_socio")]
    public string NomeSocio { get; set; } = string.Empty;

    [JsonPropertyName("cnpj_cpf_do_socio")]
    public string CnpjCpfDoSocio { get; set; } = string.Empty;

    [JsonPropertyName("codigo_qualificacao_socio")]
    public int CodigoQualificacaoSocio { get; set; }

    [JsonPropertyName("qualificacao_socio")]
    public string QualificacaoSocio { get; set; } = string.Empty;

    [JsonPropertyName("data_entrada_sociedade")]
    public DateTime? DataEntradaSociedade { get; set; }

    [JsonPropertyName("codigo_pais")]
    public int? CodigoPais { get; set; }

    [JsonPropertyName("pais")]
    public string Pais { get; set; } = string.Empty;

    [JsonPropertyName("cpf_representante_legal")]
    public string CpfRepresentanteLegal { get; set; } = string.Empty;

    [JsonPropertyName("nome_representante_legal")]
    public string NomeRepresentanteLegal { get; set; } = string.Empty;

    [JsonPropertyName("codigo_qualificacao_representante_legal")]
    public int CodigoQualificacaoRepresentanteLegal { get; set; }

    [JsonPropertyName("qualificacao_representante_legal")]
    public string QualificacaoRepresentanteLegal { get; set; } = string.Empty;

    [JsonPropertyName("codigo_faixa_etaria")]
    public int CodigoFaixaEtaria { get; set; }

    [JsonPropertyName("faixa_etaria")]
    public string FaixaEtaria { get; set; } = string.Empty;
}

public class CnaeSecundario
{
    [JsonPropertyName("codigo")]
    public long Codigo { get; set; }

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;
}
