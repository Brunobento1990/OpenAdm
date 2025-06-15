namespace OpenAdm.Application.Models.MovimentacaoDeProdutos;

public class MovimentoDeProdutoDashBoardModel
{
    public string Mes { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public IList<DadosMovimentoDeProdutoDashBoardModel> Dados { get; set; } = [];
}

public class DadosMovimentoDeProdutoDashBoardModel
{
    public int Quantidade { get; set; }
    public string Categoria { get; set; } = string.Empty;
}
