﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ItensTabelaDePrecoConfiguration : IEntityTypeConfiguration<ItensTabelaDePreco>
{
    public void Configure(EntityTypeBuilder<ItensTabelaDePreco> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DataDeCriacao)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.DataDeAtualizacao)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.Numero)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.ValorUnitario)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.HasOne(x => x.Produto)
            .WithMany(x => x.ItensTabelaDePreco)
            .HasForeignKey(x => x.ProdutoId);
    }
}
