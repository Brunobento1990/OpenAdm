using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace OpenAdm.Api.Configure;

public static class ConfigureControllerService
{
    public static void ConfigureController(this IServiceCollection services)
    {
        services.AddControllers(opt =>
        {
            opt.CacheProfiles.Add("Default300",
                new CacheProfile()
                {
                    Duration = 300,
                    VaryByHeader = "Referer"
                });
        }).AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}
