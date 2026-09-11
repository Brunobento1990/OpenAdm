using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class FuncionarioEsqueceuSenhaConfiguration : BaseEntityEmpresaConfiguration<FuncionarioEsqueceuSenha>
{
    public override void Configure(EntityTypeBuilder<FuncionarioEsqueceuSenha> builder)
    {
        builder.Property(x => x.Token).IsRequired();
        builder.Property(x => x.DataHoraExpiracao).IsRequired();
        builder.Property(x => x.Resetado).IsRequired();

        builder.HasIndex(x => x.Token).IsUnique();
        builder.HasIndex(x => x.FuncionarioId);

        builder.HasOne(x => x.Funcionario)
            .WithMany()
            .HasForeignKey(x => x.FuncionarioId)
            .OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
