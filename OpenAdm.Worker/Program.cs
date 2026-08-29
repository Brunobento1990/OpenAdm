using OpenAdm.Domain.Helpers;
using OpenAdm.Worker;

var builder = Host.CreateApplicationBuilder(args);

Criptografia.Configure(builder.Configuration["Criptografia:Key"]!, builder.Configuration["Criptografia:Iv"]!);

builder.Services
    .AddServicesApplication()
    .AddRepositories()
    .AddHttpClientInfra(builder.Configuration)
    .AddFilas(builder.Configuration)
    .InjectContext(builder.Configuration["ConnectionStrings:DefaultConnection"]!)
    .ConfigurarJobs();

var host = builder.Build();
host.Run();
