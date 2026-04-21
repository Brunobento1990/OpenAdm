using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Infra.EntityConfiguration.OpenAdm;

internal class ParcelaCobrancaConfiguration : IEntityTypeConfiguration<ParcelaCobranca>
{
    public void Configure(EntityTypeBuilder<ParcelaCobranca> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new
            {
                x.EmpresaOpenAdmId,
                x.AnoCobranca,
                x.MesCobranca,
                x.Numero
            })
            .IsUnique();

        builder.HasIndex(x => new
        {
            x.EmpresaOpenAdmId,
            x.AnoCobranca,
            x.MesCobranca
        });
        
        builder.Property(x => x.Valor)
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(x => x.ValorPago)
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(x => x.DataDeCadastro)
            .IsRequired();

        builder.Property(x => x.DataDeVencimento)
            .IsRequired();

        builder.Property(x => x.DataDePagamento)
            .IsRequired(false);

        builder.Property(x => x.Numero)
            .IsRequired();

        builder.Property(x => x.MesCobranca)
            .IsRequired();

        builder.Property(x => x.AnoCobranca)
            .IsRequired();

        builder.Property(x => x.TipoParcelaCobranca)
            .IsRequired();

        builder.HasOne(x => x.EmpresaOpenAdm)
            .WithMany()
            .HasForeignKey(x => x.EmpresaOpenAdmId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(x => x.Pago);
        builder.Ignore(x => x.Vencido);
    }
}