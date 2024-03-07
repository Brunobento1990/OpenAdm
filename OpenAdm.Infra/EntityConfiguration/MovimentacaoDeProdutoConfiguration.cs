using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenAdm.Infra.EntityConfiguration;

public class MovimentacaoDeProdutoConfiguration : IEntityTypeConfiguration<MovimentacaoDeProduto>
{
    public void Configure(EntityTypeBuilder<MovimentacaoDeProduto> builder)
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
        builder.Property(x => x.QuantidadeMovimentada)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.Property(x => x.TipoMovimentacaoDeProduto)
            .IsRequired();
        builder.Property(x => x.ProdutoId)
            .IsRequired();
        builder.HasIndex(x => x.ProdutoId);
    }
}
