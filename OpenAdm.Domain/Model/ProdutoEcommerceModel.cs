using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Model;

public class ProdutoEcommerceModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Foto { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public long Numero { get; set; }
    public ICollection<PesoTamanhoEcommerceModel> Tamanhos { get; set; } = [];
    public ICollection<PesoTamanhoEcommerceModel> Pesos { get; set; } = [];

    public static explicit operator ProdutoEcommerceModel(Produto produto)
    {
        return new ProdutoEcommerceModel
        {
            Id = produto.Id,
            Descricao = produto.Descricao,
            Referencia = produto.Referencia,
            Pesos = [],
            Tamanhos = [],
            Categoria = produto.Categoria.Descricao,
            Foto = produto.UrlFoto ?? "",
            Numero = produto.Numero
        };
    }
}

public class PesoTamanhoEcommerceModel
{
    public Guid Id { get; set; }
    public decimal? ValorUnitario { get; set; }
    public decimal? QuantidadeEstoqueDisponivel { get; set; }
    public bool TemEstoqueDisponivel { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class ResultadoProdutoEcommerceModel
{
    public ICollection<ProdutoEcommerceModel> Values { get; set; } = [];
    public int TotalPaginas { get; set; }
}