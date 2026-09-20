using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal sealed class FaturaHistoricoConfiguration : IEntityTypeConfiguration<FaturaHistorico>
{
    public void Configure(EntityTypeBuilder<FaturaHistorico> builder)
    {
        builder.ToTable("FaturaHistorico");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.DataDeCriacao)
            .IsRequired()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.FaturaId)
            .IsRequired();
        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(500);
        builder.HasIndex(x => x.FaturaId);

        builder.HasOne(x => x.Fatura)
            .WithMany(x => x.Historicos)
            .HasForeignKey(x => x.FaturaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
