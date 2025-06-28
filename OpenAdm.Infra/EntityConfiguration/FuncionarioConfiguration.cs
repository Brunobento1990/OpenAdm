using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class FuncionarioConfiguration : BaseEntityEmpresaConfiguration<Funcionario>
{
    public override void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Senha)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Telefone)
            .HasMaxLength(11);
        builder.HasIndex(x => new { x.Email, x.ParceiroId })
            .IsUnique();

        base.Configure(builder);
    }
}
