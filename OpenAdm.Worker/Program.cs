using OpenAdm.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddServicesApplication()
    .AddRepositories()
    .AddHttpClientInfra(builder.Configuration)
    .InjectContext(builder.Configuration["ConnectionStrings:DefaultConnection"]!)
    .ConfigurarJobs(builder.Configuration);

var host = builder.Build();
host.Run();