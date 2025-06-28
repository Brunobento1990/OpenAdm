using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class BannerConfiguration : BaseEntityEmpresaConfiguration<Banner>
{
    public override void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.Property(x => x.Numero)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Foto)
            .IsRequired();
        builder.Property(x => x.Ativo)
            .IsRequired();

        base.Configure(builder);
    }
}
