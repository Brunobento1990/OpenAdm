namespace OpenAdm.Domain.Model;

public sealed record PosicaoEstoqueModel(
    decimal QuantidadeFisica,
    decimal Reservado,
    bool ExigeControleDeEstoque)
{
    public bool TemEstoqueDisponivel =>
        !ExigeControleDeEstoque || Disponivel > 0;

    public decimal Disponivel =>
        !ExigeControleDeEstoque
            ? 0
            : QuantidadeFisica - Reservado;

    public bool PossuiDisponivel(decimal quantidade)
    {
        return !ExigeControleDeEstoque ||
               Disponivel >= quantidade;
    }
}