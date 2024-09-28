using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Application.Models.Tamanhos;

namespace OpenAdm.Application.Models.TabelaDePrecos;

public class ItensTabelaDePrecoViewModel : BaseModel
{
    public decimal ValorUnitarioAtacado { get; set; }
    public decimal ValorUnitarioVarejo { get; set; }
    public Guid TabelaDePrecoId { get; set; }
    public Guid ProdutoId { get; set; }
    public ProdutoViewModel Produto { get; set; } = new();
    public Guid? TamanhoId { get; set; }
    public TamanhoViewModel? Tamanho { get; set; }
    public Guid? PesoId { get; set; }
    public PesoViewModel? Peso { get; set; }

    public ItensTabelaDePrecoViewModel ToModel(ItemTabelaDePreco itensTabelaDePreco, IList<Peso> pesos, IList<Tamanho> tamanhos)
    {
        Id = itensTabelaDePreco.Id;
        DataDeCriacao = itensTabelaDePreco.DataDeCriacao;
        DataDeAtualizacao = itensTabelaDePreco.DataDeAtualizacao;
        Numero = itensTabelaDePreco.Numero;
        ValorUnitarioAtacado = itensTabelaDePreco.ValorUnitarioAtacado;
        ValorUnitarioVarejo = itensTabelaDePreco.ValorUnitarioVarejo;
        TabelaDePrecoId = itensTabelaDePreco.TabelaDePrecoId;
        ProdutoId = itensTabelaDePreco.ProdutoId;

        if (itensTabelaDePreco.Produto != null)
            Produto = new ProdutoViewModel()
                .ToModel(itensTabelaDePreco.Produto);

        if (itensTabelaDePreco.TamanhoId != null)
        {
            TamanhoId = itensTabelaDePreco.TamanhoId;
            Tamanho = tamanhos
                .Select(x => new TamanhoViewModel().ToModel(x))
                .FirstOrDefault(x => x.Id == itensTabelaDePreco.TamanhoId.Value);
        }

        if (itensTabelaDePreco.PesoId != null)
        {
            PesoId = itensTabelaDePreco.PesoId;
            Peso = pesos
                    .Select(x => new PesoViewModel().ToModel(x))
                    .FirstOrDefault(x => x.Id == itensTabelaDePreco.PesoId.Value);
        }


        return this;
    }
}
