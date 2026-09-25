using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Constants;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Data.EntityConfiguration;

public sealed class RepresentanteConfiguration : IEntityTypeConfiguration<Representante>
{
    public void Configure(EntityTypeBuilder<Representante> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DataDeCriacao).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("now()");
        builder.Property(x => x.DataDeAtualizacao).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("now()");
        builder.Property(x => x.Numero).ValueGeneratedOnAdd();
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(RepresentanteConstantes.NomeMaxLength);
        builder.Property(x => x.Cpf).HasMaxLength(RepresentanteConstantes.CpfMaxLength);
        builder.Property(x => x.Email).HasMaxLength(RepresentanteConstantes.EmailMaxLength);
        builder.Property(x => x.Telefone).HasMaxLength(RepresentanteConstantes.TelefoneMaxLength);
        builder.Property(x => x.Senha).HasMaxLength(RepresentanteConstantes.SenhaMaxLength);
        builder.Property(x => x.Ativo).HasDefaultValue(true);

        builder.HasIndex(x => x.Cpf).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Ativo);
        builder.HasIndex(x => new { x.Nome, x.Ativo });
    }
}
