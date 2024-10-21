using OpenAdm.Application.Dtos.Ceps;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Application.Interfaces;

public interface IFreteService
{
    Task<FreteViewModel> CotarFreteAsync(CotarFreteDto cotarFreteDto);
}
