using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace OpenAdm.IoC;

public static class DependencyInjectLog
{
    public static void ConfigureLog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Seq(builder.Configuration["Seq:Url"]!)
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}