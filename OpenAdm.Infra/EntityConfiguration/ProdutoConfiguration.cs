using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Pkg.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
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
        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(x => x.Descricao);
        builder.Property(x => x.EspecificacaoTecnica)
            .HasDefaultValue(null)
            .HasMaxLength(500);
        builder.Property(x => x.UrlFoto)
            .HasMaxLength(500);
        builder.Property(x => x.Referencia)
            .HasMaxLength(255)
            .HasDefaultValue(null);
        builder.HasMany(x => x.Pesos)
            .WithMany(x => x.Produtos)
            .UsingEntity<PesosProdutos>();
        builder.HasMany(x => x.Tamanhos)
            .WithMany(x => x.Produtos)
            .UsingEntity<TamanhosProdutos>();
    }
}
