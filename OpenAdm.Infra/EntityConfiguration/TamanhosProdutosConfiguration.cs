using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;
public class TamanhosProdutosConfiguration : IEntityTypeConfiguration<TamanhoProduto>
{
    public void Configure(EntityTypeBuilder<TamanhoProduto> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
