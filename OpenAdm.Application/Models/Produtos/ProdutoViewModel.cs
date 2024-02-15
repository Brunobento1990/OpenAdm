using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Entities;
using System.Text;

namespace OpenAdm.Application.Models.Produtos;

public class ProdutoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public string? EspecificacaoTecnica { get; set; }
    public string Foto { get; set; } = string.Empty;
    //public List<TamanhoViewModel>? Tamanhos { get; set; } = new();
    //public List<PesoViewModel>? Pesos { get; set; } = new();
    public Guid CategoriaId { get; set; }
    public CategoriaViewModel? Categoria { get; set; } = null!;
    public string? Referencia { get; private set; }
    public ProdutoViewModel ToModel(Produto entity)
    {

        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        Descricao = entity.Descricao;
        EspecificacaoTecnica = entity.EspecificacaoTecnica;
        Foto = Encoding.UTF8.GetString(entity.Foto);
        //Tamanhos = entity.Tamanhos.OrderBy(x => x.Numero).Select(x => new TamanhoViewModel().ForModel(x) ?? new()).ToList();
        //Pesos = entity.Pesos.OrderBy(x => x.Numero).Select(x => new PesoViewModel().ForModel(x) ?? new()).ToList();

        if (entity.Categoria != null)
            Categoria = new CategoriaViewModel().ToModel(entity.Categoria);

        CategoriaId = entity.CategoriaId;
        Referencia = entity.Referencia;
        return this;
    }
}
