using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.TabelaDePrecos;

public class TabelaDePrecoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public bool AtivaEcommerce { get; set; }

    public TabelaDePrecoViewModel ToEntity(TabelaDePreco tabelaDePreco)
    {
        Id = tabelaDePreco.Id;
        DataDeCriacao = tabelaDePreco.DataDeCriacao;
        DataDeAtualizacao = tabelaDePreco.DataDeAtualizacao;
        Numero = tabelaDePreco.Numero;
        Descricao = tabelaDePreco.Descricao;
        AtivaEcommerce = tabelaDePreco.AtivaEcommerce;

        return this;
    }
}
