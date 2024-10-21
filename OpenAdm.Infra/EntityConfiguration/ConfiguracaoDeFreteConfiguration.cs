using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class ConfiguracaoDeFreteConfiguration : IEntityTypeConfiguration<ConfiguracaoDeFrete>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoDeFrete> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CepOrigem)
            .IsRequired()
            .HasMaxLength(9);
        builder.Property(x => x.AlturaEmbalagem)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.LarguraEmbalagem)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.ComprimentoEmbalagem)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.ChaveApi)
            .IsRequired()
            .HasMaxLength(500);
    }
}
