using dotenv.net;
using OpenAdm.Api;
using OpenAdm.Api.Configure;
using OpenAdm.Api.Controllers.MinimalApis;
using OpenAdm.Api.Controllers.MinimalApis.Ecommerce;
using OpenAdm.Api.Extensions;
using OpenAdm.Api.Middlewares;
using OpenAdm.Application.DependencyInject;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Helpers;
using OpenAdm.Infra.Azure.Configuracao;
using OpenAdm.IoC;
using OpenAdm.Pdf;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

var googleClientId = VariaveisDeAmbiente.GetVariavel("GOOGLE_CLIENT_ID");
var keyJwt = VariaveisDeAmbiente.GetVariavel("JWT_KEY");
var issue = VariaveisDeAmbiente.GetVariavel("JWT_ISSUE");
var audience = VariaveisDeAmbiente.GetVariavel("JWT_AUDIENCE");
var expirate = int.Parse(VariaveisDeAmbiente.GetVariavel("JWT_EXPIRATION"));
var azureKey = VariaveisDeAmbiente.GetVariavel("AZURE_KEY");
var azureContainer = VariaveisDeAmbiente.GetVariavel("AZURE_CONTAINER");
var key = VariaveisDeAmbiente.GetVariavel("CRYP_KEY");
var iv = VariaveisDeAmbiente.GetVariavel("CRYP_IV");
var pgString = VariaveisDeAmbiente.GetVariavel("STRING_CONNECTION");
var redisString = VariaveisDeAmbiente.GetVariavel("REDIS_URL");
var urlApiCep = VariaveisDeAmbiente.GetVariavel("URL_API_CEP");
var urlApiViaCep = VariaveisDeAmbiente.GetVariavel("URL_API_VIA_CEP");
var email = VariaveisDeAmbiente.GetVariavel("EMAIL");
var servidor = VariaveisDeAmbiente.GetVariavel("SERVER");
var senha = VariaveisDeAmbiente.GetVariavel("SENHA");
var instanceName = VariaveisDeAmbiente.GetVariavel("REDIS_INSTANCENAME");
var ambiente = VariaveisDeAmbiente.GetVariavel("AMBIENTE");
var rodarMigration = VariaveisDeAmbiente.GetVariavel("RODAR_MIGRATION");
var porta = int.Parse(VariaveisDeAmbiente.GetVariavel("PORT"));

ConfiguracaoDeToken.Configure(keyJwt, issue, audience, expirate, googleClientId);
ConfigAzure.Configure(azureKey, azureContainer);
Criptografia.Configure(key, iv);
EmailConfiguracaoModel.Configure(email: email, servidor: servidor, senha: senha, porta: porta);

builder.Services.InjectCqs()
    .AddServicesApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureController();
builder.Services.InjectServices();
builder.Services.InjectCors();
builder.Services.InjectJwt(keyJwt, issue, audience);
builder.Services.InjectContext(pgString);
builder.Services.InjectRepositories(redisString, instanceName);
builder.Services
    .ConfigurarPdf()
    .InjectHttpClient(urlApiCep, urlApiViaCep,
        builder.Configuration);

builder.ConfigureLog();

var app = builder.Build();

var basePath = "/api";
app.UsePathBase(new PathString(basePath));

app.UseRouting();

app.UseCors("base");

app.AddMiddlewaresApi();

app.UseAuthorization();

app.MapControllers();

app.MaperControllerRelatorioVendaDeProduto()
    .MaperControllerHome()
    .MaperControllerCategoriaEcommerce()
    .MaperControllerBannerEcommerce()
    .MaperControllerParcelaCobranca();

app.MapEndpoints();

_ = Task.Run(async () =>
{
    try
    {
        if (rodarMigration?.ToUpper() != "TRUE")
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        var servico = scope.ServiceProvider.GetService<IMigrationService>();
        if (servico != null)
        {
            await servico.UpdateMigrationAsync(ambiente);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Falha ao executar rotinas iniciais de banco");
    }
});

app.Run();