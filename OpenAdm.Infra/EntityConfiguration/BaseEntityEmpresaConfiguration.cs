using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Infra.EntityConfiguration;

internal class BaseEntityEmpresaConfiguration<T> : BaseEntityConfiguration<T> where T : BaseEntityParceiro
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasIndex(x => x.ParceiroId);

        base.Configure(builder);
    }
}
