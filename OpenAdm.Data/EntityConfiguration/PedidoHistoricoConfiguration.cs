using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Data.EntityConfiguration;

internal sealed class PedidoHistoricoConfiguration : IEntityTypeConfiguration<PedidoHistorico>
{
    public void Configure(EntityTypeBuilder<PedidoHistorico> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Descricao).IsRequired().HasMaxLength(500);
        builder.Property(x => x.DataDeCriacao).IsRequired();
        builder.HasQueryFilter(x => !x.Pedido.Excluido);
        builder.HasOne(x => x.Pedido).WithMany(x => x.Historicos).HasForeignKey(x => x.PedidoId).OnDelete(DeleteBehavior.Cascade);
    }
}
