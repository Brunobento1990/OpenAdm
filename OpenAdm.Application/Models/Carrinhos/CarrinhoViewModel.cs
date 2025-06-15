using OpenAdm.Application.Models.Categorias;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Application.Models.Carrinhos;

public class ItemCarrinhoViewModel
{
    public Guid Id { get; set; }
    public long Numero { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? EspecificacaoTecnica { get; set; }
    public string Foto { get; set; } = string.Empty;
    public List<TamanhoCarrinhoViewModel>? Tamanhos { get; set; } = new();
    public List<PesoCarrinhoViewModel>? Pesos { get; set; } = new();
    public Guid CategoriaId { get; set; }
    public CategoriaViewModel? Categoria { get; set; } = null!;
    public string? Referencia { get; set; }
}

public class CarrinhoViewModel
{
    public IList<ItemCarrinhoViewModel> Itens { get; set; } = [];
    public EnderecoViewModel? EnderecoUsuario { get; set; }
}
