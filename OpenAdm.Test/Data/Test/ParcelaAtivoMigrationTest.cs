using Microsoft.EntityFrameworkCore;
using Moq;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Test.Data.Test;

public class ParcelaAtivoMigrationTest
{
    [Fact]
    public void DeveRegistrarMigrationDeParcelaAtivoNoEf()
    {
        var parceiro = new Mock<IParceiroAutenticado>();
        parceiro.Setup(x => x.ConnectionString)
            .Returns("Host=localhost;Database=openadm_test;Username=test;Password=test");
        using var context = new ParceiroContext(
            new DbContextOptionsBuilder<ParceiroContext>().Options, parceiro.Object);

        Assert.Contains("20260915120000_ParcelaAtivoRenegociacaoMigration",
            context.Database.GetMigrations());
    }
}
