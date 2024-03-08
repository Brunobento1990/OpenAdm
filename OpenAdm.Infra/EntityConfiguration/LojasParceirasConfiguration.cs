using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenAdm.Infra.EntityConfiguration;

public class LojasParceirasConfiguration : IEntityTypeConfiguration<LojasParceiras>
{
    public void Configure(EntityTypeBuilder<LojasParceiras> builder)
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
    }
}
