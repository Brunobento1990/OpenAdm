using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class EnderecoUsuarioConfiguration : IEntityTypeConfiguration<EnderecoUsuario>
{
    public void Configure(EntityTypeBuilder<EnderecoUsuario> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Cep)
            .IsRequired()
            .HasMaxLength(8);
        builder.Property(x => x.Uf)
            .IsRequired()
            .HasMaxLength(2);
        builder.Property(x => x.Logradouro)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Bairro)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Localidade)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Complemento)
            .HasMaxLength(255);
        builder.Property(x => x.Numero)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(x => x.Usuario)
            .WithOne(x => x.EnderecoUsuario)
            .HasForeignKey<EnderecoUsuario>(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
