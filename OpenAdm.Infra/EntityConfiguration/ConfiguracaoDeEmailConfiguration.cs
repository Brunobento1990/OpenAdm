using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ConfiguracaoDeEmailConfiguration : IEntityTypeConfiguration<ConfiguracaoDeEmail>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoDeEmail> builder)
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
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Servidor)
           .IsRequired()
           .HasMaxLength(255);
        builder.Property(x => x.Senha)
           .IsRequired()
           .HasMaxLength(255);
        builder.Property(x => x.Porta)
           .IsRequired();
        builder.Property(x => x.Ativo)
           .IsRequired();
    }
}
