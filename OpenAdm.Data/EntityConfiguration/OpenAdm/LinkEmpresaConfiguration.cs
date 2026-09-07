using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Infra.EntityConfiguration;

namespace OpenAdm.Infra.EntityConfiguration.OpenAdm;

internal sealed class LinkEmpresaConfiguration : BaseEntityConfiguration<LinkEmpresa>
{
    public override void Configure(EntityTypeBuilder<LinkEmpresa> builder)
    {
        builder.Property(x => x.Url).HasMaxLength(350).IsRequired();
        builder.HasIndex(x => x.Url).IsUnique();
        builder.HasIndex(x => x.EmpresaId).IsUnique();
        builder.HasOne(x => x.Empresa).WithOne(x => x.Link).HasForeignKey<LinkEmpresa>(x => x.EmpresaId);
        base.Configure(builder);
    }
}
