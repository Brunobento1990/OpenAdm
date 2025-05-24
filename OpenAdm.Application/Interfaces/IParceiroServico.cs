using OpenAdm.Application.Dtos.Parceiros;
using OpenAdm.Application.Models.Parceiros;

namespace OpenAdm.Application.Interfaces;

public interface IParceiroServico
{
    Task<ParceiroViewModel> ObterParceiroAutenticadoAsync();
    Task<ParceiroViewModel> EditarAsync(ParceiroDto parceiroDto);
    Task<bool> ExcluirTelefoneAsync(Guid telefoneId);
    Task<bool> ExcluirRedeSocialAsync(Guid redeSocialId);
}
