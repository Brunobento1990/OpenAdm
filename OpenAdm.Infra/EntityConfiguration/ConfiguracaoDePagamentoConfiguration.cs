using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class ConfiguracaoDePagamentoConfiguration : IEntityTypeConfiguration<ConfiguracaoDePagamento>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoDePagamento> builder)
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
        builder.Property(x => x.Numero)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.PublicKey)
            .IsRequired()
            .HasMaxLength(2000);
        builder.Property(x => x.AccessToken)
            .IsRequired()
            .HasMaxLength(2000);
    }
}
