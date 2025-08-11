using Microsoft.EntityFrameworkCore;
using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities.OpenAdm;
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

    public async Task UpdateMigrationAsync(string ambiente)
    {
        await _appDbContext.Database.MigrateAsync();

        if (ambiente.Equals("develop", StringComparison.OrdinalIgnoreCase))
        {
            var empresa = await _appDbContext
                .Parceiros
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RazaoSocial == ambiente);

            if (empresa == null)
            {
                var empresaOpenAdm = new EmpresaOpenAdm(
                    id: Guid.NewGuid(),
                    dataDeCriacao: DateTime.UtcNow,
                    dataDeAtualizacao: DateTime.UtcNow,
                    numero: 0,
                    ativo: true,
                    urlEcommerce: "http://localhost:3000",
                    urlAdmin: "http://localhost:7154",
                    connectionString: Criptografia.Encrypt("User ID=postgres; Password=1234; Host=localhost; Port=4449; Database=open-adm-cliente-develop; Pooling=true;"));

                empresa = new Domain.Entities.Parceiro(
                    id: Guid.NewGuid(),
                    dataDeCriacao: DateTime.UtcNow,
                    dataDeAtualizacao: DateTime.UtcNow,
                    numero: 0,
                    razaoSocial: ambiente,
                    nomeFantasia: ambiente,
                    cnpj: "00000000000191",
                    logo: null,
                    empresaOpenAdmId: empresaOpenAdm.Id)
                {
                    EmpresaOpenAdm = empresaOpenAdm
                };

                await _appDbContext.Parceiros.AddAsync(empresa);
            }

            var funcionario = await _appDbContext
                .Funcionarios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == "brunobentocaina@gmail.com");

            if (funcionario == null)
            {
                funcionario = new Domain.Entities.Funcionario(
                    id: Guid.NewGuid(),
                    dataDeCriacao: DateTime.UtcNow,
                    dataDeAtualizacao: DateTime.UtcNow,
                    numero: 0,
                    email: "brunobentocaina@gmail.com",
                    senha: PasswordAdapter.GenerateHash("1234"),
                    nome: "Bruno Bento",
                    telefone: "11999999999",
                    avatar: null,
                    ativo: true,
                    parceiroId: empresa.EmpresaOpenAdmId);

                await _appDbContext.Funcionarios.AddAsync(funcionario);
            }

            await _appDbContext.SaveChangesAsync();
        }

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
