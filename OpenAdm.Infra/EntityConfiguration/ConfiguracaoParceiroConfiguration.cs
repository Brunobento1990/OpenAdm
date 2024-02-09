using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;
using System.Reflection.Emit;

namespace OpenAdm.Infra.EntityConfiguration;

public class ConfiguracaoParceiroConfiguration : IEntityTypeConfiguration<ConfiguracaoParceiro>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoParceiro> builder)
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
        builder.Property(x => x.Dominio)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.ConexaoDb)
            .IsRequired()
            .HasMaxLength(1000);
        builder.HasIndex(x => x.Dominio);
    }
}
