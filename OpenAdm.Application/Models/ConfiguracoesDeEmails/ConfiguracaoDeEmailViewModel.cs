using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Helpers;

namespace OpenAdm.Application.Models.ConfiguracoesDeEmails;

public class ConfiguracaoDeEmailViewModel : BaseModel
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Servidor { get; set; } = string.Empty;
    public int Porta { get; set; }
    public bool Ativo { get; set; }

    public ConfiguracaoDeEmailViewModel ToModel(ConfiguracaoDeEmail configuracaoDeEmail)
    {
        Id = configuracaoDeEmail.Id;
        DataDeCriacao = configuracaoDeEmail.DataDeCriacao;
        DataDeAtualizacao = configuracaoDeEmail.DataDeAtualizacao;
        Numero = configuracaoDeEmail.Numero;
        Email = configuracaoDeEmail.Email;
        Senha = Criptografia.Decrypt(configuracaoDeEmail.Senha);
        Servidor = configuracaoDeEmail.Servidor;
        Porta = configuracaoDeEmail.Porta;
        Ativo = configuracaoDeEmail.Ativo;

        return this;
    }
}
