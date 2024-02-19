using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class PesosProdutosConfiguration : IEntityTypeConfiguration<PesosProdutos>
{
    public void Configure(EntityTypeBuilder<PesosProdutos> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
