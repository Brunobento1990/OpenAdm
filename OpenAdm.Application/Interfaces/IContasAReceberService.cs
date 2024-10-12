using OpenAdm.Application.Dtos.ContasAReceberDto;

namespace OpenAdm.Application.Interfaces;

public interface IContasAReceberService
{
    Task CriarContasAReceberAsync(CriarContasAReceberDto contasAReceberDto);
}
