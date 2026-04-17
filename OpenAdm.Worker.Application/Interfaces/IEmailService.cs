using OpenAdm.Worker.Application.DTOs;

namespace OpenAdm.Worker.Application.Interfaces;

public interface IEmailService
{
    Task<bool> EnviarAsync(EnviarEmailDTO enviarEmailDTO);
}