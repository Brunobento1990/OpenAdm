using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Data.EntityConfiguration.OpenAdm;

internal class SessaoUsuarioConfiguration : BaseEntityConfiguration<SessaoUsuario>
{
    public override void Configure(EntityTypeBuilder<SessaoUsuario> builder)
    {
        builder.Property(x => x.EnderecoIp)
            .HasMaxLength(SessaoUsuarioConfig.MaxLengthEnderecoIp);

        builder.Property(x => x.UserAgent)
            .HasMaxLength(SessaoUsuarioConfig.MaxLengthUserAgent);

        builder.Property(x => x.SistemaOperacional)
            .HasMaxLength(SessaoUsuarioConfig.MaxLengthSistemaOperacional);

        builder.Property(x => x.Navegador)
            .HasMaxLength(SessaoUsuarioConfig.MaxLengthNavegador);

        builder.Property(x => x.Dispositivo)
            .HasMaxLength(SessaoUsuarioConfig.MaxLengthDispositivo);

        builder.HasIndex(x => new
        {
            x.UsuarioId,
            x.ParceiroId,
            x.EhFuncionario,
            x.RevogadoEm,
            x.ExpiraEm
        });

        builder.Ignore(x => x.Numero);
        
        base.Configure(builder);
    }
}
