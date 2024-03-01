using OpenAdm.Application.Dtos.ConfiguracoesDeEmails;
using OpenAdm.Application.Models.ConfiguracoesDeEmails;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracoesDeEmailService
{
    Task<ConfiguracaoDeEmailViewModel> CreateConfiguracoesDeEmailAsync(CreateConfiguracoesDeEmailDto createConfiguracoesDeEmailDto);
}
