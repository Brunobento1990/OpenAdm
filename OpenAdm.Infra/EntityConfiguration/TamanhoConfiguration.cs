﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class TamanhoConfiguration : IEntityTypeConfiguration<Tamanho>
{
    public void Configure(EntityTypeBuilder<Tamanho> builder)
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
        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(x => x.Descricao);
    }
}
