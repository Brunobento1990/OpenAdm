namespace OpenAdm.Application.Dtos.ConfiguracoesDePagamentos;

public class ConfiguracaoDePagamentoCreateOrUpdate
{
    public string PublicKey { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public bool CobrarCpf { get; set; }
    public bool CobrarCnpj { get; set; }
}
