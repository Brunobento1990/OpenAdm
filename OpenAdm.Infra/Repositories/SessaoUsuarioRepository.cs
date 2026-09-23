using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Infra.Repositories;

public class SessaoUsuarioRepository(AppDbContext appDbContext) : ISessaoUsuarioRepository
{
    public async Task AdicionarAsync(SessaoUsuario sessao)
    {
        await appDbContext.SessoesUsuarios.AddAsync(sessao);
    }

    public Task<SessaoUsuario?> ObterAsync(
        Guid sessaoId,
        Guid usuarioId,
        Guid parceiroId,
        bool ehFuncionario)
    {
        return appDbContext
            .SessoesUsuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == sessaoId &&
                x.UsuarioId == usuarioId &&
                x.ParceiroId == parceiroId &&
                x.EhFuncionario == ehFuncionario);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }

    public async Task DerrubarSessaoAsync(Guid sessaoId)
    {
        await appDbContext.SessoesUsuarios
            .Where(x => x.Id == sessaoId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.RevogadoEm, DateTime.UtcNow));
    }

    public async Task DerrubarSessaoUsuarioIdAsync(Guid usuarioId)
    {
        await appDbContext.SessoesUsuarios
            .Where(x => x.UsuarioId == usuarioId && x.RevogadoEm == null)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.RevogadoEm, DateTime.UtcNow));
    }
}