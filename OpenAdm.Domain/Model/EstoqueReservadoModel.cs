namespace OpenAdm.Domain.Model;

public sealed record EstoqueReservadoModel(
    Guid ProdutoId,
    Guid? PesoId,
    Guid? TamanhoId,
    decimal QuantidadeReservada);