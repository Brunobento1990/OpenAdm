using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ConfiguracoesDePedidoConfiguration : IEntityTypeConfiguration<ConfiguracoesDePedido>
{
    public void Configure(EntityTypeBuilder<ConfiguracoesDePedido> builder)
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
        builder.Property(x => x.EmailDeEnvio)
            .IsRequired()
            .HasMaxLength(255);
    }
}
