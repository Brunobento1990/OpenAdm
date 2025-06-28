using dotenv.net;
using OpenAdm.Api;
using OpenAdm.Api.Configure;
using OpenAdm.Api.Middlewares;
using OpenAdm.Application.DependencyInject;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Helpers;
using OpenAdm.Infra.Azure.Configuracao;
using OpenAdm.IoC;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

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
var urlDiscord = VariaveisDeAmbiente.GetVariavel("URL_DISCORD");
var urlApiCep = VariaveisDeAmbiente.GetVariavel("URL_API_CEP");
var urlApiViaCep = VariaveisDeAmbiente.GetVariavel("URL_API_VIA_CEP");
var urlApiMercadoPago = VariaveisDeAmbiente.GetVariavel("URL_API_MERCADO_PAGO");
var email = VariaveisDeAmbiente.GetVariavel("EMAIL");
var servidor = VariaveisDeAmbiente.GetVariavel("SERVER");
var senha = VariaveisDeAmbiente.GetVariavel("SENHA");
var urlConsultaCnpj = VariaveisDeAmbiente.GetVariavel("ULR_CONSULTA_CNPJ");
var instanceName = VariaveisDeAmbiente.GetVariavel("REDIS_INSTANCENAME");
var porta = int.Parse(VariaveisDeAmbiente.GetVariavel("PORT"));
var rodarMigration = VariaveisDeAmbiente.GetVariavel("RODAR_MIGRATION");

ConfiguracaoDeToken.Configure(keyJwt, issue, audience, expirate);
ConfigAzure.Configure(azureKey, azureContainer);
Criptografia.Configure(key, iv);
EmailConfiguracaoModel.Configure(email: email, servidor: servidor, senha: senha, porta: porta);

builder.Services.AddServicesApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureController();
builder.Services.InjectServices();
builder.Services.InjectCors();
builder.Services.InjectJwt(keyJwt, issue, audience);
builder.Services.InjectContext(pgString);
builder.Services.InjectRepositories(redisString, instanceName);
builder.Services.InjectHttpClient(urlDiscord, urlApiCep, urlApiMercadoPago, urlConsultaCnpj, urlApiViaCep);

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

var basePath = "/api";
app.UsePathBase(new PathString(basePath));

app.UseRouting();

if (VariaveisDeAmbiente.GetVariavel("AMBIENTE").Equals("develop"))
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI();
}

app.UseCors("base");

app.AddMiddlewaresApi();

app.UseAuthorization();

app.MapControllers();

if (rodarMigration?.ToLower() == "true")
{
    using var scope = app.Services.CreateScope();
    var migrationServico = scope.ServiceProvider.GetService<IMigrationService>();
    if (migrationServico != null)
    {
        await migrationServico.UpdateMigrationAsync();
    }
}

app.Run();
