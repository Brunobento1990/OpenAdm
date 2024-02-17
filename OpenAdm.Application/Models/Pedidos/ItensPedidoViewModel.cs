using OpenAdm.Application.Models.Pesos;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Application.Models.Tamanhos;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Pedidos;

public class ItensPedidoViewModel : BaseModel
{
    public Guid? PesoId { get; set; }
    public PesoViewModel? Peso { get; set; }
    public Guid? TamanhoId { get; set; }
    public TamanhoViewModel? Tamanho { get; set; }
    public Guid ProdutoId { get; set; }
    public ProdutoViewModel Produto { get; set; } = null!;
    public Guid PedidoId { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorTotal { get; set; }
    public ItensPedidoViewModel ToModel(ItensPedido entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        Numero = entity.Numero;
        PesoId = entity.PesoId;
        TamanhoId = entity.TamanhoId;
        ProdutoId = entity.ProdutoId;
        PedidoId = entity.PedidoId;

        if (entity.Peso != null)
        {
            Peso = new PesoViewModel().ToModel(entity.Peso);
        }

        if (entity.Tamanho != null)
        {
            Tamanho = new TamanhoViewModel().ToModel(entity.Tamanho);
        }

        Produto = new ProdutoViewModel().ToModel(entity.Produto) ?? new();
        ValorUnitario = entity.ValorUnitario;
        ValorTotal = entity.ValorTotal;
        Quantidade = entity.Quantidade;

        return this;
    }
}
