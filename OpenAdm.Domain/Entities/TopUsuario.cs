﻿using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class TopUsuario : BaseEntity
{
    public TopUsuario(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        decimal totalCompra,
        int totalPedidos,
        Guid usuarioId,
        string usuario)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        TotalCompra = totalCompra;
        TotalPedidos = totalPedidos;
        UsuarioId = usuarioId;
        Usuario = usuario;
    }

    public void Update(decimal totalCompra, int totalPedidos)
    {
        TotalCompra += totalCompra;
        TotalPedidos += totalPedidos;
    }

    public decimal TotalCompra { get; private set; }
    public int TotalPedidos { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string Usuario { get; private set; }
}
