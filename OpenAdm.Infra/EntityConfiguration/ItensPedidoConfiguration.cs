using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ItensPedidoConfiguration : IEntityTypeConfiguration<ItensPedido>
{
    public void Configure(EntityTypeBuilder<ItensPedido> builder)
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
        builder.Ignore(x => x.ValorTotal);
        builder.Property(x => x.ValorUnitario)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.Property(x => x.Quantidade)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.HasOne(x => x.Tamanho)
            .WithMany(x => x.ItensPedido)
            .HasForeignKey(x => x.TamanhoId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Peso)
            .WithMany(x => x.ItensPedido)
            .HasForeignKey(x => x.PesoId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Produto)
            .WithMany(x => x.ItensPedido)
            .HasForeignKey(x => x.ProdutoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
