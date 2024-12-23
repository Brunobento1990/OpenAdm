using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Infra.EntityConfiguration;

internal class TransacaoFinanceiraConfiguration : IEntityTypeConfiguration<TransacaoFinanceira>
{
    public void Configure(EntityTypeBuilder<TransacaoFinanceira> builder)
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
        builder.Property(x => x.Valor)
            .IsRequired()
            .HasPrecision(12, 2);
        builder.Property(x => x.Observacao)
            .HasMaxLength(500);

        builder.HasOne(x => x.Parcela)
            .WithMany(x => x.Transacoes)
            .HasForeignKey(x => x.ParcelaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
