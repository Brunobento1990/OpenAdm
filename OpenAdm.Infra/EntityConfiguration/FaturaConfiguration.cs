using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class FaturaConfiguration : IEntityTypeConfiguration<Fatura>
{
    public void Configure(EntityTypeBuilder<Fatura> builder)
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
        builder.HasIndex(x => x.Tipo);
        builder.Ignore(x => x.Total);
        builder.HasMany(x => x.Parcelas)
            .WithOne(x => x.Fatura)
            .HasForeignKey(x => x.FaturaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
