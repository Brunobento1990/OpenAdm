using Domain.Pkg.Cryptography;
using Domain.Pkg.Pdfs.Configure;
using dotenv.net;
using Microsoft.OpenApi.Models;
using OpenAdm.Api;
using OpenAdm.Api.Configure;
using OpenAdm.Api.Middlewares;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Infra.Azure.Configuracao;
using OpenAdm.IoC;

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
var rabbitMqString = VariaveisDeAmbiente.GetVariavel("MENSAGERIA_URI");
var urlDiscord = VariaveisDeAmbiente.GetVariavel("URL_DISCORD");

ConfiguracaoDeToken.Configure(keyJwt, issue, audience, expirate);
ConfigAzure.Configure(azureKey, azureContainer);
CryptographyGeneric.Configure(key, iv);

builder.Services.AddResponseCaching();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureController();
builder.Services.InjectServices();
builder.Services.InjectCors();
builder.Services.InjectJwt(keyJwt, issue, audience);
builder.Services.InjectContext(pgString);
builder.Services.InjectRepositories(redisString);
builder.Services.InjectHttpClient(urlDiscord);
builder.Services.InjectMensageria(rabbitMqString);

ConfigurePdfQuest.ConfigureQuest();

var app = builder.Build();

var basePath = "/api/v1";
app.UsePathBase(new PathString(basePath));

app.UseRouting();

if (VariaveisDeAmbiente.GetVariavel("AMBIENTE").Equals("develop"))
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
        });
    });
    app.UseSwaggerUI();
}
app.UseResponseCaching();

app.UseCors("base");

app.AddMiddlewaresApi();

app.UseAuthorization();

app.MapControllers();

app.Run();
