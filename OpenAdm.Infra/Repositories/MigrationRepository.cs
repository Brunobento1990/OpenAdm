using Microsoft.EntityFrameworkCore;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Helpers;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.Repositories;

public class MigrationRepository : IMigrationService
{
    private readonly AppDbContext _appDbContext;

    public MigrationRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task UpdateMigrationAsync()
    {
        await _appDbContext.Database.MigrateAsync();

        var stringDeConexoes = await _appDbContext
            .Empresas
            .AsNoTracking()
            .Where(x => x.Ativo)
            .Select(x => x.ConnectionString)
            .Distinct()
            .ToListAsync();

        foreach (var stringConexao in stringDeConexoes)
        {
            var options = new DbContextOptionsBuilder<ParceiroContext>().Options;
            using var appDbContext = new ParceiroContext(options, new ParceiroAutenticadoV2()
            {
                ConnectionString = Criptografia.Decrypt(stringConexao)
            });

            await appDbContext.Database.MigrateAsync();
        }
    }
}
