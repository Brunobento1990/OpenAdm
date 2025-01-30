using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class AcessoEcommerceConfiguration : IEntityTypeConfiguration<AcessoEcommerce>
{
    public void Configure(EntityTypeBuilder<AcessoEcommerce> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(x => x.DataDeCriacao);
    }
}
