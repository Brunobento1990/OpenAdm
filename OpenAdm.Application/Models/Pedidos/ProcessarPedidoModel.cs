﻿using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.Pedidos;

public class ProcessarPedidoModel
{
    public string EmailEnvio { get; set; } = string.Empty;
    public Pedido Pedido { get; set; } = null!;
}
