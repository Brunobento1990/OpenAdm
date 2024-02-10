using Microsoft.Extensions.DependencyInjection;

namespace OpenAdm.IoC;

public static class DependencyInjectCors
{
    public static void InjectCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: "base",
                              policy =>
                              {
                                  policy.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader();
                              });
        });
    }
}
