﻿using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Dtos.ContasAReceberDto;

public class GerarPagamentoDto
{
    public Guid PedidoId { get; set; }
    public MeioDePagamentoEnum MeioDePagamento { get; set; }
}
