using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities;

public sealed class Fatura : BaseEntity
{
    public Fatura(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        StatusFaturaEnum status,
        Guid usuarioId,
        Guid? pedidoId,
        DateTime? dataDeFechamento,
        TipoFaturaEnum tipo)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Status = status;
        UsuarioId = usuarioId;
        PedidoId = pedidoId;
        DataDeFechamento = dataDeFechamento;
        Tipo = tipo;
    }

    public StatusFaturaEnum Status { get; private set; }
    public TipoFaturaEnum Tipo { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; set; } = null!;
    public Guid? PedidoId { get; private set; }
    public Pedido? Pedido { get; set; }
    public DateTime? DataDeFechamento { get; private set; }
    public IList<Parcela> Parcelas { get; set; } = [];
    public decimal Total { get { return Parcelas.Sum(x => x.Valor); } }

    public void Fechar()
    {
        DataDeFechamento = DateTime.Now;
        Status = StatusFaturaEnum.Paga;
        DataDeAtualizacao = DateTime.Now;
    }

    public void PagaParcialmente()
    {
        Status = StatusFaturaEnum.Paga_Parcialmente;
        DataDeAtualizacao = DateTime.Now;
    }

    public static Fatura NovaContasAReceber(
        Guid usuarioId,
        Guid? pedidoId,
        decimal total,
        int quantidadeDeParcelas,
        DateTime primeiroVencimento,
        MeioDePagamentoEnum? meioDePagamento,
        decimal? desconto,
        string? observacao,
        string? idExterno,
        TipoFaturaEnum tipo)
    {
        var fatura = new Fatura(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            status: StatusFaturaEnum.Aberta,
            usuarioId: usuarioId,
            pedidoId: pedidoId,
            dataDeFechamento: null,
            tipo: tipo);

        var valorDaParcela = total / quantidadeDeParcelas;

        for (int i = 1; i <= quantidadeDeParcelas; i++)
        {
            var novoVencimento = i == 1 ? primeiroVencimento : primeiroVencimento.AddMonths(i);
            fatura.Parcelas.Add(Parcela.NovaFatura(
                dataDeVencimento: novoVencimento,
                numeroDaFatura: i,
                meioDePagamento: null,
                valor: valorDaParcela,
                desconto: desconto,
                observacao: observacao,
                faturaId: fatura.Id,
                idExterno: idExterno));
        }

        var totalValidacao = fatura.Parcelas.Sum(x => x.Valor);

        if (totalValidacao > total)
        {
            var diferenca = totalValidacao - total;
            var primeiraParcela = fatura.Parcelas.FirstOrDefault();
            primeiraParcela?.TirarDiferenca(diferenca);
        }
        else if (totalValidacao < total)
        {
            var diferenca = total - totalValidacao;
            var primeiraParcela = fatura.Parcelas.FirstOrDefault();
            primeiraParcela?.AdicionarDiferenca(diferenca);
        }

        return fatura;
    }
}
