namespace OpenAdm.Domain.Model;

public class ProdutoEcommerceModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Foto { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public long Numero { get; set; }
    public ICollection<PesoTamanhoEcommerceModel> Tamanhos { get; set; } = [];
    public ICollection<PesoTamanhoEcommerceModel> Pesos { get; set; } = [];
}

public class PesoTamanhoEcommerceModel
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class ResultadoProdutoEcommerceModel
{
    public IEnumerable<ProdutoEcommerceModel> Produtos { get; set; } = [];
    public int QuantidadeDePagina { get; set; }
}