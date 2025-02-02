using Microsoft.EntityFrameworkCore;
using OpenAdm.Infra.Context;

namespace OpenAdm.Test.Memory;

public class ParceiroContextMemory
{
    private readonly ParceiroContext _parceiroContext;

    public ParceiroContextMemory()
    {
        var options = new DbContextOptionsBuilder<ParceiroContext>();
        options.UseInMemoryDatabase("iscaslune");
        _parceiroContext = new ParceiroContext(options.Options);
    }

    public static ParceiroContextMemory Init() => new();
    public ParceiroContext Build() => _parceiroContext;
}
