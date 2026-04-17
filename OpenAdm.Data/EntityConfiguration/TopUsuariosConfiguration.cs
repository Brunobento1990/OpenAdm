using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenAdm.Infra.EntityConfiguration;

internal class TopUsuariosConfiguration : BaseEntityEmpresaConfiguration<TopUsuario>
{
    public override void Configure(EntityTypeBuilder<TopUsuario> builder)
    {
        builder.Property(x => x.TotalCompra)
            .HasPrecision(12, 2)
            .IsRequired();
        builder.Property(x => x.TotalPedidos)
            .IsRequired();
        builder.Property(x => x.Usuario)
            .HasMaxLength(255)
            .IsRequired();

        base.Configure(builder);
    }
}
