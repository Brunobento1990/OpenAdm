using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class ConfiguracoesDePedidoConfiguration : BaseEntityEmpresaConfiguration<ConfiguracoesDePedido>
{
    public override void Configure(EntityTypeBuilder<ConfiguracoesDePedido> builder)
    {
        builder.Property(x => x.EmailDeEnvio)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.WhatsApp)
            .HasMaxLength(20);
        builder.Property(x => x.PedidoMinimoAtacado)
            .HasPrecision(12, 2);
        builder.Property(x => x.PedidoMinimoVarejo)
            .HasPrecision(12, 2);

        builder.Property(x => x.VendaDeProdutoComEstoque)
            .IsRequired()
            .HasDefaultValue(false);

        base.Configure(builder);
    }
}
