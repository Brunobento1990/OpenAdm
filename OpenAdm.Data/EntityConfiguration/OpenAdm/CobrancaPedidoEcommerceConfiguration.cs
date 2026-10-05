using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Data.EntityConfiguration.OpenAdm;

internal class CobrancaPedidoEcommerceConfiguration : BaseEntityConfiguration<CobrancaPedidoEcommerce>
{
    public override void Configure(EntityTypeBuilder<CobrancaPedidoEcommerce> builder)
    {
        builder.Property(x => x.Total)
            .HasPrecision(12, 2);

        builder.HasIndex(x => x.Ativo);
        builder.HasIndex(x => x.PedidoId).IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasQueryFilter(x => !x.Pedido.Excluido);

        builder.HasOne(x => x.Pedido)
            .WithOne(x => x.Cobranca)
            .HasForeignKey<CobrancaPedidoEcommerce>(x => x.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(x => new { x.Status, x.Ativo });

        base.Configure(builder);
    }
}
