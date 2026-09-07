using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Data.EntityConfiguration;

internal sealed class LinkBioItemConfiguration : BaseEntityConfiguration<LinkBioItem>
{
    public override void Configure(EntityTypeBuilder<LinkBioItem> builder)
    {
        builder.Property(x => x.Titulo).IsRequired();
        builder.Property(x => x.Url).IsRequired();
        builder.Property(x => x.Icone).IsRequired(false);
        builder.Property(x => x.Ordem).IsRequired();
        builder.Property(x => x.Ativo).IsRequired();
        builder.HasIndex(x => new { x.LinkBioConfiguracaoId, x.Ordem });
        base.Configure(builder);
    }
}
