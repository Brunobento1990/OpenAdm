namespace OpenAdm.Application.Dtos.ItensPedidos;

public class RelatorioProducaoDto
{
    public IList<Guid> PedidosIds { get; set; } = [];
    public IList<Guid> ProdutosIds { get; set; } = [];
    public IList<Guid> PesosIds { get; set; } = [];
    public IList<Guid> TamanhosIds { get; set; } = [];
}
