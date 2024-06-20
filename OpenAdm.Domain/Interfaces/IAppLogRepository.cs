using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IAppLogRepository
{
    Task AddAsync(AppLog appLog);
}
