using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ProdutosMaisVendidosConfiguration : IEntityTypeConfiguration<ProdutosMaisVendidos>
{
    public void Configure(EntityTypeBuilder<ProdutosMaisVendidos> builder)
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
        builder.Property(x => x.ProdutoId)
            .IsRequired();
        builder.Property(x => x.QuantidadeProdutos)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.Property(x => x.QuantidadePedidos)
            .IsRequired()
            .HasPrecision(12, 2);
    }
}
