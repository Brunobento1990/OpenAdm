using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Infra.EntityConfiguration.OpenAdm;

internal class EmpresaOpenAdmConfiguration : BaseEntityConfiguration<EmpresaOpenAdm>
{
    public override void Configure(EntityTypeBuilder<EmpresaOpenAdm> builder)
    {
        builder.HasIndex(x => x.TipoParcelaCobranca);
        builder.HasIndex(x => x.Ativo);

        base.Configure(builder);
    }
}
