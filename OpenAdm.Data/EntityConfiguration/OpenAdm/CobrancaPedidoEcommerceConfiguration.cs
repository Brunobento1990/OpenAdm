using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Data.EntityConfiguration.OpenAdm;

internal class CobrancaPedidoEcommerceConfiguration : BaseEntityEmpresaConfiguration<CobrancaPedidoEcommerce>
{
    public override void Configure(EntityTypeBuilder<CobrancaPedidoEcommerce> builder)
    {
        builder.Property(x => x.Total)
            .HasPrecision(12, 2);

        builder.HasIndex(x => x.Ativo);
        builder.HasIndex(x => x.PedidoId);
        builder.HasIndex(x => x.Status);
        
        builder.HasIndex(x => new { x.ParceiroId, x.Status, x.Ativo });

        base.Configure(builder);
    }
}