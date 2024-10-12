using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

namespace OpenAdm.Infra.Repositories;

public sealed class ContasAReceberRepository : GenericRepository<ContasAReceber>, IContasAReceberRepository
{
    public ContasAReceberRepository(ParceiroContext parceiroContext) : base(parceiroContext)
    {
    }
}
