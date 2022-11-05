using Habr.BL.Interfaces.Services.BackgroundJobs;
using Habr.BL.Interfaces.Services.V1;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Habr.BL.Services.BackgroundJobs;

public class BackgroundJobService : IBackgroundJobService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IRecurringJobManager _recurringJobManager;

    public BackgroundJobService(IServiceProvider serviceProvider, IRecurringJobManager recurringJobManager)
    {
        _serviceProvider = serviceProvider;
        _recurringJobManager = recurringJobManager;
    }

    public void CreateRatingBackgroundJob()
    {
        _recurringJobManager.AddOrUpdate(
            nameof(IRatingService.RecalculateRatingForPosts),
            () => CallUpdateRatings(),
            Cron.Daily
        );
    }

    public async Task CallUpdateRatings()
    {
        using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var ratingService = serviceScope.ServiceProvider.GetService<IRatingService>()!;
            await ratingService.RecalculateRatingForPosts();
        }
    }
}