using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ConfiguracaoParceiroConfiguration : IEntityTypeConfiguration<ConfiguracaoParceiro>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoParceiro> builder)
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
        builder.Property(x => x.DominioSiteAdm)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.DominioSiteEcommerce)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.ConexaoDb)
            .IsRequired()
            .HasMaxLength(1000);
        builder.HasIndex(x => x.DominioSiteAdm)
            .IsUnique(true);
        builder.HasIndex(x => x.DominioSiteEcommerce)
            .IsUnique(true);
    }
}
