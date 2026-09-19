namespace OpenAdm.Domain.Model.Pedidos;

public sealed class ResumoMensalPedidoModel
{
    public int QuantidadePedidos { get; set; }
    public decimal ValorTotalVendido { get; set; }
    public decimal QuantidadeItensVendidos { get; set; }
    public ICollection<ResumoMensalCategoriaModel> Categorias { get; set; } = [];
}

public sealed class ResumoMensalCategoriaModel
{
    public Guid CategoriaId { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public decimal QuantidadeItensVendidos { get; set; }
}
