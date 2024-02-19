using Domain.Pkg.Entities;
using OpenAdm.Application.Models.Produtos;
using System.Text;

namespace OpenAdm.Application.Models.Categorias;

public class CategoriaViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public string? Foto { get; set; }
    public List<ProdutoViewModel>? Produtos { get; set; }

    public CategoriaViewModel ToModel(Categoria entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        Descricao = entity.Descricao;
        Foto = entity.Foto != null ? Encoding.UTF8.GetString(entity.Foto) : null;
        Produtos = entity.Produtos.Select(x => new ProdutoViewModel().ToModel(x)).ToList();

        return this;
    }
}
