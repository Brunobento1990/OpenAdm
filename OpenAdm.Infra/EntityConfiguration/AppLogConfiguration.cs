using Domain.Pkg.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenAdm.Infra.EntityConfiguration;

public class AppLogConfiguration : IEntityTypeConfiguration<AppLog>
{
    public void Configure(EntityTypeBuilder<AppLog> builder)
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
        builder.Property(x => x.Origem)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(x => x.Latitude)
            .HasMaxLength(100);
        builder.Property(x => x.Longitude)
            .HasMaxLength(100);
        builder.Property(x => x.Host)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(x => x.Ip)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.Path)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.Erro)
            .HasMaxLength(1000);
        builder.Property(x => x.StatusCode)
            .IsRequired();
        builder.Property(x => x.LogLevel)
            .IsRequired();
        builder.HasIndex(x => x.LogLevel);
        builder.HasIndex(x => x.StatusCode);
    }
}
