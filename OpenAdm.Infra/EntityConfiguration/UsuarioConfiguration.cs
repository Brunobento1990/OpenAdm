using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
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
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Senha)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(x => x.Telefone)
            .HasMaxLength(15);
        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}
