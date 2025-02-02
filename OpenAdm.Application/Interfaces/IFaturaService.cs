using OpenAdm.Application.Dtos.FaturasDtos;
using OpenAdm.Application.Models.ContasAReceberModel;

namespace OpenAdm.Application.Interfaces;

public interface IFaturaService
{
    Task CriarContasAReceberAsync(CriarFaturaDto contasAReceberDto);
    Task VerificarFechamentoAsync(Guid id);
    Task<FaturaViewModel> CriarAdmAsync(FaturaCriarAdmDto faturaCriarAdmDto);
    Task<FaturaViewModel> GetCompletaAsync(Guid id);
    Task<FaturaViewModel> GetByIdAsync(Guid id);
}
