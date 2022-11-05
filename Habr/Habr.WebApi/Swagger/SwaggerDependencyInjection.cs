using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Habr.WebApi.Swagger;

public static class SwaggerDependencyInjection
{
    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services, IApiVersionDescriptionProvider provider)
    {
        services.AddSwaggerGen(option =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                option.SwaggerDoc($"v{description.ApiVersion.ToString()}", new OpenApiInfo()
                {
                    Title = "Swagger with auth",
                    Description = "Demo Swagger API",
                    Version = description.ApiVersion.ToString()
                });
            }
            
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
        });

        return services;
    }

    public static WebApplication UseCustomSwagger(this WebApplication app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
                c.RoutePrefix = string.Empty;
            }
        });

        return app;
    }
}