using Microsoft.OpenApi.Models;

namespace ResourceManager.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerWithAuthentication(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // c.SwaggerDoc("v1", new OpenApiInfo {
            //     Title = "JWTToken_Auth_API", Version = "v1"
            // });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n "
                              + "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n"
                              + "Example: \"Bearer 1safsfsdfdfd\"",
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

        return services;
    }
}
