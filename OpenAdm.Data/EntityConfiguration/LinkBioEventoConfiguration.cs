using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Data.EntityConfiguration;

internal sealed class LinkBioEventoConfiguration : BaseEntityConfiguration<LinkBioEvento>
{
    public override void Configure(EntityTypeBuilder<LinkBioEvento> builder)
    {
        builder.Property(x => x.Tipo).IsRequired();
        builder.HasIndex(x => new { x.EmpresaId, x.DataDeCriacao });
        builder.HasIndex(x => new { x.LinkBioConfiguracaoId, x.Tipo });
        builder.HasOne(x => x.Empresa).WithMany().HasForeignKey(x => x.EmpresaId);
        builder.HasOne(x => x.LinkBioConfiguracao).WithMany().HasForeignKey(x => x.LinkBioConfiguracaoId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        builder.HasOne(x => x.LinkBioItem).WithMany().HasForeignKey(x => x.LinkBioItemId)
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
        base.Configure(builder);
    }
}
