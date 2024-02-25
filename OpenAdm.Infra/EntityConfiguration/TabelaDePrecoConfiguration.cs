using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class TabelaDePrecoConfiguration : IEntityTypeConfiguration<TabelaDePreco>
{
    public void Configure(EntityTypeBuilder<TabelaDePreco> builder)
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
        builder.Property(x => x.AtivaEcommerce)
            .IsRequired();
        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasMany(x => x.ItensTabelaDePreco)
            .WithOne(x => x.TabelaDePreco)
            .HasForeignKey(x => x.TabelaDePrecoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}