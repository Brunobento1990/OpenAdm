using OpenAdm.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenAdm.Infra.EntityConfiguration;

internal class LojasParceirasConfiguration : BaseEntityEmpresaConfiguration<LojaParceira>
{
    public override void Configure(EntityTypeBuilder<LojaParceira> builder)
    {
        builder.Property(x => x.NomeFoto)
            .HasMaxLength(500);
        builder.Property(x => x.Foto)
            .HasMaxLength(500);
        builder.Property(x => x.Instagram)
            .HasMaxLength(500);
        builder.Property(x => x.Facebook)
            .HasMaxLength(500);
        builder.Property(x => x.Endereco)
            .HasMaxLength(500);
        builder.Property(x => x.Contato)
            .HasMaxLength(20);
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(255);

        base.Configure(builder);
    }
}
