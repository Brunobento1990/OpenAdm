using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ItensTabelaDePrecoConfiguration : IEntityTypeConfiguration<ItemTabelaDePreco>
{
    public void Configure(EntityTypeBuilder<ItemTabelaDePreco> builder)
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
        builder.Property(x => x.ValorUnitarioAtacado)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.Property(x => x.ValorUnitarioVarejo)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.HasOne(x => x.Produto)
            .WithMany(x => x.ItensTabelaDePreco)
            .HasForeignKey(x => x.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
