using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Infra.EntityConfiguration.OpenAdm;

internal class EventoAplicacaoConfiguration : IEntityTypeConfiguration<EventoAplicacao>
{
    public void Configure(EntityTypeBuilder<EventoAplicacao> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Mensagem)
            .HasMaxLength(1000);

        builder.HasIndex(x => new { x.QuantidadeDeTentativa, x.Finalizado });
    }
}