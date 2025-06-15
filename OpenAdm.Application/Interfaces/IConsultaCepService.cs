using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Application.Interfaces;

public interface IConsultaCepService
{
    Task<EnderecoViewModel> ConsultarAsync(string cep);
}
