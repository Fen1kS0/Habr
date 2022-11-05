using System.Reflection;
using FluentValidation;
using Habr.BL.Interfaces.Services;
using Habr.BL.Interfaces.Services.BackgroundJobs;
using Habr.BL.Interfaces.Services.V1;
using Habr.BL.Interfaces.Services.V2;
using Habr.BL.Interfaces.Services.V3;
using Habr.BL.Services;
using Habr.BL.Services.BackgroundJobs;
using Habr.BL.Services.V1;
using Habr.BL.Services.V2;
using Habr.BL.Services.V3;
using Microsoft.Extensions.DependencyInjection;

namespace Habr.BL;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IRatingService, RatingService>();
        
        services.AddScoped<IPostServiceV2, PostServiceV2>();
        services.AddScoped<IPostServiceV3, PostServiceV3>();
        
        services.AddSingleton<IBackgroundJobService, BackgroundJobService>();
        
        return services;
    }
    
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}