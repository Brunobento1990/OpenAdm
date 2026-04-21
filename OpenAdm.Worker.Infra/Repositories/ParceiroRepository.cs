using Microsoft.EntityFrameworkCore;
using OpenAdm.Data.Context;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Worker.Infra.Repositories;

public class ParceiroRepository : IParceiroRepository
{
    private readonly AppDbContext _appDbContext;

    public ParceiroRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task AddEndereco(EnderecoParceiro enderecoParceiro)
    {
        throw new NotImplementedException();
    }

    public Task AdicionarRedesSociaisAsync(IList<RedeSocial> redesSociais)
    {
        throw new NotImplementedException();
    }

    public Task AdicionarTelefonesAsync(IList<TelefoneParceiro> telefones)
    {
        throw new NotImplementedException();
    }

    public async Task<Parceiro?> ObterPorEmpresaOpenAdmIdAsync(Guid empresaOpenAdmId)
    {
        return await _appDbContext
            .Parceiros
            .AsNoTracking()
            .Include(x => x.Telefones)
            .Include(x => x.EnderecoParceiro)
            .FirstOrDefaultAsync(x => x.EmpresaOpenAdmId == empresaOpenAdmId);
    }

    public Task<RedeSocial?> ObterRedeSocialAsync(Guid redeSocialId)
    {
        throw new NotImplementedException();
    }

    public Task<TelefoneParceiro?> ObterTelefoneAsync(Guid telefoneId)
    {
        throw new NotImplementedException();
    }

    public void RemoverEndereco(EnderecoParceiro enderecoParceiro)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Parceiro>> ObterParaCobrancaAsync()
    {
        return await _appDbContext
            .Parceiros
            .Include(x => x.EmpresaOpenAdm)
            .Where(x => x.EmpresaOpenAdm.TipoParcelaCobranca == TipoParcelaCobrancaEnum.Mensal)
            .ToListAsync();
    }

    public void RemoverRedeSocial(RedeSocial redeSocial)
    {
        throw new NotImplementedException();
    }

    public void RemoverTelefone(TelefoneParceiro telefoneParceiro)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public void Update(Parceiro parceiro)
    {
        throw new NotImplementedException();
    }

    public void UpdateRedesSociais(IList<RedeSocial> redesSociais)
    {
        throw new NotImplementedException();
    }

    public void UpdateTelefones(IList<TelefoneParceiro> telefones)
    {
        throw new NotImplementedException();
    }
}