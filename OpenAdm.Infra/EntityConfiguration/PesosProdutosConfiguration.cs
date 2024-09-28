using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class PesosProdutosConfiguration : IEntityTypeConfiguration<PesoProduto>
{
    public void Configure(EntityTypeBuilder<PesoProduto> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
