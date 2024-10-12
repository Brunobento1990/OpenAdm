﻿using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.ContasAReceberDto;

public class CriarContasAReceberDto
{
    public Guid UsuarioId { get; set; }
    public Guid? PedidoId { get; set; }
    public decimal Total { get; set; }
    public decimal? Desconto { get; set; }
    public int QuantidadeDeParcelas { get; set; }
    public DateTime DataDoPrimeiroVencimento { get; set; }
    public MeioDePagamentoEnum? MeioDePagamento { get; set; }
    public string? Observacao { get; set; }
}
