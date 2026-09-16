using Microsoft.EntityFrameworkCore;
using Moq;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Data.Test;

public class FaturaHistoricoConfigurationTest
{
    [Fact]
    public void DeveConfigurarRelacionamentoComFatura()
    {
        var parceiro = new Mock<IParceiroAutenticado>();
        parceiro.Setup(x => x.ConnectionString)
            .Returns("Host=localhost;Database=openadm_test;Username=test;Password=test");
        using var context = new ParceiroContext(
            new DbContextOptionsBuilder<ParceiroContext>().Options, parceiro.Object);

        var entidade = context.Model.FindEntityType(typeof(FaturaHistorico));
        Assert.NotNull(entidade);
        Assert.Equal("FaturaHistorico", entidade.GetTableName());
        Assert.Equal(500, entidade.FindProperty(nameof(FaturaHistorico.Descricao))!.GetMaxLength());

        var chaveEstrangeira = Assert.Single(entidade.GetForeignKeys());
        Assert.Equal(typeof(Fatura), chaveEstrangeira.PrincipalEntityType.ClrType);
        Assert.Equal(DeleteBehavior.Cascade, chaveEstrangeira.DeleteBehavior);
    }
}
