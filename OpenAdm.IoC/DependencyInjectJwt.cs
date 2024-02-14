using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OpenAdm.IoC;

public static class DependencyInjectJwt
{
    public static void InjectJwt(this IServiceCollection services, string key, string issue, string audience)
    {
        services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme).
            AddJwtBearer(options =>
             options.TokenValidationParameters = OptionsToken(key, issue, audience));
    }

    private static TokenValidationParameters OptionsToken(string key, string issue, string audience)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = audience,
            ValidIssuer = issue,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(key))
        };
    }
}
