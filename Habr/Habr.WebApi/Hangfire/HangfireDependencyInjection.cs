using Habr.BL.Interfaces.Services.BackgroundJobs;
using Hangfire;
using Hangfire.SqlServer;

namespace Habr.WebApi.Hangfire;

public static class HangfireDependencyInjection
{
    public static IServiceCollection AddCustomHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));
        
        return services;
    }
    
    public static WebApplication RunBackgroundJobs(this WebApplication app)
    {
        app.Services.GetService<IBackgroundJobService>()?.CreateRatingBackgroundJob();

        return app;
    }
}