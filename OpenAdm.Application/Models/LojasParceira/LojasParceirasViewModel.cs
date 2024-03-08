using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.LojasParceira;

public class LojasParceirasViewModel : BaseModel
{
    public string Nome { get; set; } = string.Empty;
    public string? Foto { get; set; }
    public string? Instagram { get; set; }
    public string? Facebook { get; set; }
    public string? Endereco { get; set; }
    public string? Contato { get; set; }

    public LojasParceirasViewModel ToModel(LojasParceiras lojasParceiras)
    {
        Id = lojasParceiras.Id;
        DataDeAtualizacao = lojasParceiras.DataDeAtualizacao;
        Numero = lojasParceiras.Numero;
        DataDeCriacao = lojasParceiras.DataDeCriacao;
        Foto = lojasParceiras.Foto;
        Instagram = lojasParceiras.Instagram;
        Facebook = lojasParceiras.Facebook;
        Endereco = lojasParceiras.Endereco;
        Contato = lojasParceiras.Contato;
        Nome = lojasParceiras.Nome;

        return this;
    }
}
