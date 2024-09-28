using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.TabelaDePrecos;

public class TabelaDePrecoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public bool AtivaEcommerce { get; set; }
    public IList<ItensTabelaDePrecoViewModel> ItensTabelaDePreco { get; set; }
        = new List<ItensTabelaDePrecoViewModel>();

    public TabelaDePrecoViewModel ToModel(TabelaDePreco tabelaDePreco)
    {
        Id = tabelaDePreco.Id;
        DataDeCriacao = tabelaDePreco.DataDeCriacao;
        DataDeAtualizacao = tabelaDePreco.DataDeAtualizacao;
        Numero = tabelaDePreco.Numero;
        Descricao = tabelaDePreco.Descricao;
        AtivaEcommerce = tabelaDePreco.AtivaEcommerce;

        if(tabelaDePreco.ItensTabelaDePreco != null && tabelaDePreco.ItensTabelaDePreco.Count > 0)
        {
            ItensTabelaDePreco = tabelaDePreco.ItensTabelaDePreco.Select(x => new ItensTabelaDePrecoViewModel().ToModel(x, new List<Peso>(), new List<Tamanho>())).ToList();
        }

        return this;
    }
}
