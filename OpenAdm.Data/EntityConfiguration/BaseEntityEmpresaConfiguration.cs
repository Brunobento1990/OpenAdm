using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Infra.EntityConfiguration;

internal class BaseEntityEmpresaConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntityParceiro
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DataDeCriacao)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.DataDeAtualizacao)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()");

        builder.HasIndex(x => x.Numero);
        builder.HasIndex(x => x.ParceiroId);
    }
}
