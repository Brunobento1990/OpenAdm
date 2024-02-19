using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ParceiroConfiguration : IEntityTypeConfiguration<Parceiro>
{
    public void Configure(EntityTypeBuilder<Parceiro> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DataDeCriacao)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.DataDeAtualizacao)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.Numero)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Cnpj)
            .IsRequired()
            .HasMaxLength(14);
        builder.Property(x => x.RazaoSocial)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.NomeFantasia)
            .IsRequired()
        .HasMaxLength(255);

        builder.HasOne(parceiro => parceiro.ConfiguracaoDbParceiro)
            .WithOne(configuracao => configuracao.Parceiro)
            .HasForeignKey<ConfiguracaoParceiro>(configuracao => configuracao.ParceiroId);
    }
}
