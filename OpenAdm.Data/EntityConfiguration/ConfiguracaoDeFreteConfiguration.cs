using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class ConfiguracaoDeFreteConfiguration : BaseEntityEmpresaConfiguration<ConfiguracaoDeFrete>
{
    public override void Configure(EntityTypeBuilder<ConfiguracaoDeFrete> builder)
    {
        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.CepOrigem)
            .HasMaxLength(8);

        base.Configure(builder);
    }
}