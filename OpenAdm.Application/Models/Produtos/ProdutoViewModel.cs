using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Application.Models.Tamanhos;

namespace OpenAdm.Application.Models.Produtos;

public class ProdutoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public string? EspecificacaoTecnica { get; set; }
    public string? Foto { get; set; } = string.Empty;
    public List<TamanhoViewModel>? Tamanhos { get; set; } = new();
    public List<PesoViewModel>? Pesos { get; set; } = new();
    public Guid CategoriaId { get; set; }
    public CategoriaViewModel? Categoria { get; set; } = null!;
    public string? Referencia { get; private set; }
    public decimal? Peso { get; set; }
    public ProdutoViewModel ToModel(Produto entity)
    {
        Foto = entity.UrlFoto;
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        Descricao = entity.Descricao;
        EspecificacaoTecnica = entity.EspecificacaoTecnica;
        Tamanhos = entity
            .Tamanhos
            .OrderBy(x => x.Numero)
            .Select(x => new TamanhoViewModel().ToModel(x))
            .ToList();
        Pesos = entity
            .Pesos
            .OrderBy(x => x.Numero)
            .Select(x => new PesoViewModel().ToModel(x))
            .ToList();

        if (entity.Categoria != null)
            Categoria = new CategoriaViewModel().ToModel(entity.Categoria);

        CategoriaId = entity.CategoriaId;
        Referencia = entity.Referencia;
        return this;
    }
}
