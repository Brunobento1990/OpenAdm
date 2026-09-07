using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Data.EntityConfiguration;

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

        builder.HasIndex(x => new { x.ParceiroId, x.Ativo });

        builder.Property(x => x.Ativo)
            .HasDefaultValue(true);

        base.Configure(builder);
    }
}