namespace OpenAdm.Application.Models.Home;

public sealed class ResumoMensalHomeViewModel
{
    public int Mes { get; set; }
    public int AnoAtual { get; set; }
    public int AnoAnterior { get; set; }
    public VariacaoMensalViewModel<int> QuantidadePedidos { get; set; } = new();
    public VariacaoMensalViewModel<decimal> ValorTotalVendido { get; set; } = new();
    public VariacaoMensalViewModel<decimal> QuantidadeItensVendidos { get; set; } = new();
    public ICollection<ResumoMensalCategoriaViewModel> Categorias { get; set; } = [];
}

public sealed class ResumoMensalCategoriaViewModel
{
    public Guid CategoriaId { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public VariacaoMensalViewModel<decimal> QuantidadeItensVendidos { get; set; } = new();
}

public sealed class VariacaoMensalViewModel<T> where T : struct
{
    public T Atual { get; set; }
    public T AnoAnterior { get; set; }
    public T Variacao { get; set; }
    public decimal VariacaoPercentual { get; set; }
}
