using Domain.Pkg.Entities;
using OpenAdm.Application.Models.Produtos;

namespace OpenAdm.Application.Models.Tamanhos;

public class TamanhoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public PrecoProdutoViewModel? PrecoProdutoView { get; set; }
    public TamanhoViewModel ToModel(Tamanho entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        Descricao = entity.Descricao;

        return this;
    }
}
