namespace OpenAdm.Application.Dtos.MovimentosDeProdutos;

public class RelatorioMovimentoDeProdutoDto
{
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public IList<Guid> ProdutosId { get; set; } = [];
    public IList<Guid> PesosId { get; set; } = [];
    public IList<Guid> TamanhosId { get; set; } = [];
    public IList<Guid> CategoriasId { get; set; } = [];
}
