using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class ConfiguracaoDePagamentoConfiguration : BaseEntityEmpresaConfiguration<ConfiguracaoDePagamento>
{
    public override void Configure(EntityTypeBuilder<ConfiguracaoDePagamento> builder)
    {
        builder.Property(x => x.PublicKey)
            .IsRequired()
            .HasMaxLength(2000);
        builder.Property(x => x.AccessToken)
            .IsRequired()
            .HasMaxLength(2000);
        builder.Property(x => x.UrlWebHook)
            .HasMaxLength(2000);

        base.Configure(builder);
    }
}
