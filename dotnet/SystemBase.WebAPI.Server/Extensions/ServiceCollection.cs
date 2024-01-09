using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SystemBase;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMapster(this IServiceCollection services, params Assembly[] assemblies)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assemblies);
        return services
            .AddSingleton(config)
            .AddSingleton<MapsterMapper.IMapper, ServiceMapper>();
    }

    public static IServiceCollection AddSystemBaseAuthentication(this IServiceCollection services, JWTOptions options)
    {
        services.AddAuthentication(authOptions => {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions => {
            bearerOptions.TokenValidationParameters = new()
            {
                IssuerSigningKey = options.GetSymmetricSecurityKey(),
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                ValidateIssuer = options.ValidateIssuer,
                ValidateAudience = options.ValidateAudience,
                ValidateLifetime = options.ValidateLifetime,
                ValidateIssuerSigningKey = options.ValidateIssuerSigningKey
            };
        });
        return services;
    }
}