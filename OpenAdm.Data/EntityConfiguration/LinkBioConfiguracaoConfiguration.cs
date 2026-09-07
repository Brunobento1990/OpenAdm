using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Data.EntityConfiguration;

internal sealed class LinkBioConfiguracaoConfiguration : BaseEntityConfiguration<LinkBioConfiguracao>
{
    public override void Configure(EntityTypeBuilder<LinkBioConfiguracao> builder)
    {
        builder.Property(x => x.Titulo).IsRequired();
        builder.Property(x => x.BackgroundImage);
        builder.Property(x => x.NomeBackgroundImage);
        builder.Property(x => x.Ativo).IsRequired();
        builder.HasIndex(x => x.EmpresaId).IsUnique();
        builder.HasOne(x => x.Empresa).WithMany().HasForeignKey(x => x.EmpresaId);
        builder.HasMany(x => x.Links).WithOne(x => x.LinkBioConfiguracao)
            .HasForeignKey(x => x.LinkBioConfiguracaoId).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        base.Configure(builder);
    }
}
