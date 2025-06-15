using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Infra.EntityConfiguration.OpenAdm;

internal class RedeSocialConfiguration : BaseEntityConfiguration<RedeSocial>
{
    public override void Configure(EntityTypeBuilder<RedeSocial> builder)
    {
        builder.Property(x => x.Link)
            .IsRequired()
            .HasMaxLength(500);

        base.Configure(builder);
    }
}
