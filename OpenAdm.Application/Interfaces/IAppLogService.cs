using OpenAdm.Application.Dtos.Logs;

namespace OpenAdm.Application.Interfaces;

public interface IAppLogService
{
    Task CreateAppLogAsync(CreateAppLog createAppLog);
}
