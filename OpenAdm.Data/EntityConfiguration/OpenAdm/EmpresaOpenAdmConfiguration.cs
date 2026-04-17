using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Infra.EntityConfiguration.OpenAdm;

internal class EmpresaOpenAdmConfiguration : BaseEntityConfiguration<EmpresaOpenAdm>
{
    public override void Configure(EntityTypeBuilder<EmpresaOpenAdm> builder)
    {
        builder.Property(x => x.UrlEcommerce)
            .HasMaxLength(350)
            .IsRequired();

        builder.Property(x => x.UrlAdmin)
            .HasMaxLength(350)
            .IsRequired();

        builder.HasIndex(x => x.UrlEcommerce);
        builder.HasIndex(x => x.UrlAdmin);
        builder.HasIndex(x => x.Ativo);
        builder.HasIndex(x => new { x.UrlAdmin, x.UrlEcommerce, x.Ativo });
        builder.HasIndex(x => new { x.UrlAdmin, x.UrlEcommerce }).IsUnique();

        base.Configure(builder);
    }
}
