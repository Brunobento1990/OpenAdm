using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Infra.EntityConfiguration.OpenAdm;

internal class TelefoneParceiroConfiguration : IEntityTypeConfiguration<TelefoneParceiro>
{
    public void Configure(EntityTypeBuilder<TelefoneParceiro> builder)
    {
        builder.Property(x => x.Telefone)
            .IsRequired()
            .HasMaxLength(14);
    }
}
