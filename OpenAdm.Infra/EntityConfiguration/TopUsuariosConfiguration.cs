using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenAdm.Infra.EntityConfiguration;

public sealed class TopUsuariosConfiguration : IEntityTypeConfiguration<TopUsuarios>
{
    public void Configure(EntityTypeBuilder<TopUsuarios> builder)
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
        builder.Property(x => x.TotalCompra)
            .HasPrecision(12, 2)
            .IsRequired();
        builder.Property(x => x.TotalPedidos)
            .IsRequired();
        builder.Property(x => x.Usuario)
            .HasMaxLength(255)
            .IsRequired();
    }
}
