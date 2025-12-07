using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenAdm.Application.Adapters;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Helpers;
using OpenAdm.Infra.Context;
using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.Repositories;

public class MigrationRepository : IMigrationService
{
    private readonly AppDbContext _appDbContext;
    private readonly IConfiguration _configuration;

    public MigrationRepository(AppDbContext appDbContext, IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _configuration = configuration;
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
                    connectionString: Criptografia.Encrypt(
                        "User ID=postgres; Password=1234; Host=localhost; Port=4045; Database=open-adm-cliente-develop; Pooling=true;"));

                empresa = new Parceiro(
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
                funcionario = new Funcionario(
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

            if (_configuration["AtualizarEstoqueReservado"]?.ToUpper() == "TRUE")
            {
                var pedidosEmAberto = await appDbContext
                    .Pedidos
                    .Include(x => x.ItensPedido)
                    .Where(x => x.StatusPedido == StatusPedido.Aberto)
                    .ToListAsync();

                try
                {
                    foreach (var pedido in pedidosEmAberto)
                    {
                        var produtosIds = pedido.ItensPedido.Select(x => x.ProdutoId).ToList();

                        var estoques = await
                            appDbContext
                                .Estoques
                                .Where(x => produtosIds.Contains(x.ProdutoId))
                                .OrderByDescending(x => x.DataDeAtualizacao)
                                .ToListAsync();

                        var addEstoques = new List<Estoque>();
                        var updateEstoques = new List<Estoque>();

                        foreach (var item in pedido.ItensPedido)
                        {
                            var estoque = estoques
                                .FirstOrDefault(x =>
                                    x.ProdutoId == item.ProdutoId &&
                                    x.PesoId == item.PesoId &&
                                    x.TamanhoId == item.TamanhoId);

                            if (estoque == null)
                            {
                                estoque = addEstoques.FirstOrDefault(x =>
                                    x.ProdutoId == item.ProdutoId &&
                                    x.PesoId == item.PesoId &&
                                    x.TamanhoId == item.TamanhoId);

                                if (estoque == null)
                                {
                                    addEstoques.Add(Estoque.NovoEstoque(quantidade: 0, produtoId: item.ProdutoId,
                                        tamanhoId: item.TamanhoId,
                                        pesoId: item.PesoId));
                                    continue;
                                }

                                estoque.ReservaEstoque(item.Quantidade);
                                continue;
                            }

                            estoque.ReservaEstoque(item.Quantidade);

                            if (!updateEstoques.Any(x =>
                                    x.ProdutoId == item.ProdutoId &&
                                    x.PesoId == item.PesoId &&
                                    x.TamanhoId == item.TamanhoId))
                            {
                                updateEstoques.Add(estoque);
                            }
                        }

                        appDbContext.UpdateRange(updateEstoques);
                        await appDbContext.AddRangeAsync(addEstoques);
                        
                        await appDbContext.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}