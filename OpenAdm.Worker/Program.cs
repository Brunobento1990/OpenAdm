using OpenAdm.Worker;
using OpenAdm.Pdf;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddServicesApplication()
    .AddRepositories()
    .ConfigurarPdf()
    .AddHttpClientInfra(builder.Configuration)
    .InjectContext(builder.Configuration["ConnectionStrings:DefaultConnection"]!)
    .ConfigurarJobs(builder.Configuration);

var host = builder.Build();
host.Run();