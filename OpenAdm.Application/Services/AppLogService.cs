using Domain.Pkg.Entities;
using OpenAdm.Application.Dtos.Logs;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class AppLogService : IAppLogService
{
    private readonly IAppLogRepository _appLogRepository;

    public AppLogService(IAppLogRepository appLogRepository)
    {
        _appLogRepository = appLogRepository;
    }

    public async Task CreateAppLogAsync(CreateAppLog createAppLog)
    {
        var appLog = new AppLog(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero: 0,
            origem: createAppLog.Origem,
            host: createAppLog.Host,
            ip: createAppLog.Ip,
            path: createAppLog.Path,
            erro: createAppLog.Erro,
            statusCode: createAppLog.StatusCode,
            logLevel: createAppLog.LogLevel,
            latitude: createAppLog.Latitude,
            longitude: createAppLog.Longitude,
            xApi: createAppLog.XApi);

        await _appLogRepository.AddAsync(appLog);
    }
}
