using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace OpenAdm.Api.Configure;

public static class ConfigureControllerService
{
    public static void ConfigureController(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}
