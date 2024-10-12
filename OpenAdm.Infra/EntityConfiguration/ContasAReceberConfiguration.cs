using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class ContasAReceberConfiguration : IEntityTypeConfiguration<ContasAReceber>
{
    public void Configure(EntityTypeBuilder<ContasAReceber> builder)
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
        builder.Ignore(x => x.Total);
        builder.HasMany(x => x.Faturas)
            .WithOne(x => x.ContasAReceber)
            .HasForeignKey(x => x.ContasAReceberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
