using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Helpers;

namespace OpenAdm.Application.Models.ConfiguracoesDePagamentos;

public class ConfiguracaoDePagamentoViewModel
{
    public string PublicKey { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public bool CobrarCpf { get; set; }
    public bool CobrarCnpj { get; set; }

    public static explicit operator ConfiguracaoDePagamentoViewModel(ConfiguracaoDePagamento configuracaoDePagamento)
    {
        return new ConfiguracaoDePagamentoViewModel()
        {
            AccessToken = Criptografia.Decrypt(configuracaoDePagamento.AccessToken),
            CobrarCnpj = configuracaoDePagamento.CobrarCnpj,
            CobrarCpf = configuracaoDePagamento.CobrarCpf,
            PublicKey = Criptografia.Decrypt(configuracaoDePagamento.PublicKey)
        };
    }
}
