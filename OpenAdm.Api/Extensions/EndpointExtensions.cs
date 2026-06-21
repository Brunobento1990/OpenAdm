using OpenAdm.Api.Configure;

namespace OpenAdm.Api.Extensions;

public static class EndpointExtensions
{
    public static void MapEndpoints(
        this WebApplication app
    )
    {
        var endpoints = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(x =>
                typeof(IEndpoint).IsAssignableFrom(x) &&
                x is { IsAbstract: false, IsInterface: false });

        foreach (var endpoint in endpoints)
        {
            var instance =
                (IEndpoint)Activator.CreateInstance(endpoint)!;

            instance.MapEndpoints(app);
        }
    }
}