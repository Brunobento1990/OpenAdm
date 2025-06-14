using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Domain.Interfaces;

public interface IParceiroRepository
{
    Task<Parceiro?> ObterPorEmpresaOpenAdmIdAsync(Guid empresaOpenAdmId);
    void Update(Parceiro parceiro);
    Task<RedeSocial?> ObterRedeSocialAsync(Guid redeSocialId);
    Task<TelefoneParceiro?> ObterTelefoneAsync(Guid telefoneId);
    Task SaveChangesAsync();
    void RemoverTelefone(TelefoneParceiro telefoneParceiro);
    void RemoverRedeSocial(RedeSocial redeSocial);
    Task AdicionarRedesSociaisAsync(IList<RedeSocial> redesSociais);
    Task AdicionarTelefonesAsync(IList<TelefoneParceiro> telefones);
    void UpdateTelefones(IList<TelefoneParceiro> telefones);
    void UpdateRedesSociais(IList<RedeSocial> redesSociais);
    Task AddEndereco(EnderecoParceiro enderecoParceiro);
    void RemoverEndereco(EnderecoParceiro enderecoParceiro);
}
