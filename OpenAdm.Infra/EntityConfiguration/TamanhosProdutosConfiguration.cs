using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;
public class TamanhosProdutosConfiguration : IEntityTypeConfiguration<TamanhosProdutos>
{
    public void Configure(EntityTypeBuilder<TamanhosProdutos> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
