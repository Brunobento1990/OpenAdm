using Microsoft.EntityFrameworkCore;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public class ParceiroRepository : IParceiroRepository
{
    private readonly AppDbContext _appDbContext;

    public ParceiroRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddEndereco(EnderecoParceiro enderecoParceiro)
    {
        await _appDbContext.EnderecoParceiro.AddAsync(enderecoParceiro);
    }

    public async Task AdicionarRedesSociaisAsync(IList<RedeSocial> redesSociais)
    {
        await _appDbContext.RedesSociais.AddRangeAsync(redesSociais);
    }

    public async Task AdicionarTelefonesAsync(IList<TelefoneParceiro> telefones)
    {
        await _appDbContext.TelefonesParceiro.AddRangeAsync(telefones);
    }

    public async Task<Parceiro?> ObterPorEmpresaOpenAdmIdAsync(Guid empresaOpenAdmId)
    {
        return await _appDbContext
            .Parceiros
            .Include(x => x.RedesSociais)
            .Include(x => x.Telefones)
            .Include(x => x.EnderecoParceiro)
            .FirstOrDefaultAsync(x => x.EmpresaOpenAdmId == empresaOpenAdmId);
    }

    public async Task<RedeSocial?> ObterRedeSocialAsync(Guid redeSocialId)
    {
        return await _appDbContext
            .RedesSociais
            .FirstOrDefaultAsync(x => x.Id == redeSocialId);
    }

    public async Task<TelefoneParceiro?> ObterTelefoneAsync(Guid telefoneId)
    {
        return await _appDbContext
            .TelefonesParceiro
            .FirstOrDefaultAsync(x => x.Id == telefoneId);
    }

    public void RemoverEndereco(EnderecoParceiro enderecoParceiro)
    {
        _appDbContext.EnderecoParceiro.Remove(enderecoParceiro);
    }

    public void RemoverRedeSocial(RedeSocial redeSocial)
    {
        _appDbContext.RedesSociais.Remove(redeSocial);
    }

    public void RemoverTelefone(TelefoneParceiro telefoneParceiro)
    {
        _appDbContext.TelefonesParceiro.Remove(telefoneParceiro);
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public void Update(Parceiro parceiro)
    {
        _appDbContext.Update(parceiro);
    }

    public void UpdateRedesSociais(IList<RedeSocial> redesSociais)
    {
        _appDbContext.RedesSociais.UpdateRange(redesSociais);
    }

    public void UpdateTelefones(IList<TelefoneParceiro> telefones)
    {
        _appDbContext.TelefonesParceiro.UpdateRange(telefones);
    }
}
