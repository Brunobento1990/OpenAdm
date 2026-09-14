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
        return appDbContext.SessoesUsuarios.FirstOrDefaultAsync(x =>
            x.Id == sessaoId &&
            x.UsuarioId == usuarioId &&
            x.ParceiroId == parceiroId &&
            x.EhFuncionario == ehFuncionario);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await appDbContext.SaveChangesAsync();
    }
}
