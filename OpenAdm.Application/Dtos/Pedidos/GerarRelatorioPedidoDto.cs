namespace OpenAdm.Application.Dtos.Pedidos;

public class GerarRelatorioPedidoDto
{
    public GerarRelatorioPedidoDto(
        DateTime dataInicial,
        DateTime dataFinal,
        string? logo,
        decimal total)
    {
        DataInicial = dataInicial;
        DataFinal = dataFinal;
        Logo = logo;
        Total = total;
    }
    public DateTime DataInicial { get; private set; }
    public DateTime DataFinal { get; private set; }
    public string? Logo { get; private set; }
    public decimal Total { get; private set; }
    public IList<RelatorioItensPedidoDto> RelatorioItensPedidoDto { get; set; } = new List<RelatorioItensPedidoDto>();
}

public class RelatorioItensPedidoDto
{
    public RelatorioItensPedidoDto(
        long numero,
        string cliente,
        decimal quantidade,
        decimal total,
        DateTime dataDeCadastro)
    {
        Numero = numero;
        Cliente = cliente;
        Quantidade = quantidade;
        Total = total;
        DataDeCadastro = dataDeCadastro;
    }

    public long Numero { get; private set; }
    public string Cliente { get; private set; }
    public decimal Quantidade { get; private set; }
    public decimal Total { get; private set; }
    public DateTime DataDeCadastro { get; private set; }
}
