using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using OpenAdm.Api;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Infra.Azure.Configuracao;
using OpenAdm.IoC;
using QuestPDF.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllers(opt =>
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
builder.Services.AddResponseCaching();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Header de autorização JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[espaço] e o seu token.\r\n\r\nExamplo: \'Bearer 12345abcdef\'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var key = VariaveisDeAmbiente.GetVariavel("JWT_KEY");
var issue = VariaveisDeAmbiente.GetVariavel("JWT_ISSUE");
var audience = VariaveisDeAmbiente.GetVariavel("JWT_AUDIENCE");
var expirate = int.Parse(VariaveisDeAmbiente.GetVariavel("JWT_EXPIRATION"));
var azureKey = VariaveisDeAmbiente.GetVariavel("AZURE_KEY");
var azureContainer = VariaveisDeAmbiente.GetVariavel("AZURE_CONTAINER");

builder.Services.InjectJwt(key, issue, audience);
ConfiguracaoDeToken.Configure(key, issue, audience, expirate);
ConfigAzure.Configure(azureKey, azureContainer);
builder.Services.InjectContext(VariaveisDeAmbiente.GetVariavel("STRING_CONNECTION"));
builder.Services.InjectRepositories(VariaveisDeAmbiente.GetVariavel("REDIS_URL"));
builder.Services.InjectServices();
builder.Services.InjectCors();
builder.Services.InjectHttpClient(VariaveisDeAmbiente.GetVariavel("URL_DISCORD"));
builder.Services.InjectMensageria(VariaveisDeAmbiente.GetVariavel("MENSAGERIA_URI"));

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

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("base");

app.MapControllers();

app.Run();
