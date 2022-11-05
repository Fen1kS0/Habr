using Habr.BL;
using Habr.DataAccess;
using Habr.DataAccess.Helpers;
using Habr.WebApi.Auth;
using Habr.WebApi.Hangfire;
using Habr.WebApi.Swagger;
using Hangfire;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NLog.Web;

namespace Habr.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddLocalization(opt => opt.ResourcesPath = builder.Configuration["ResourcesPath"]);
        
        builder.Services.AddCustomController();
        builder.Services.AddCustomAuthentication(builder.Configuration);
        builder.Services.AddCustomAuthorization();
        
        builder.Services.AddDataContext(builder.Configuration);
        builder.Services.AddRepositories();
        builder.Services.AddUnitOfWork();

        builder.Services.AddServices();
        builder.Services.AddMappings();
        builder.Services.AddValidators();
        
        builder.Services.AddCustomApiVersioning();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

        builder.Services.AddCustomHangfire(builder.Configuration);
        builder.Services.AddHangfireServer();
        
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.Information);
        builder.Host.UseNLog();

        var app = builder.Build();

#if DEBUG
        SeedData.Seed(new DataContext());
        DatabaseHelper.CreateDatabase("Habr.Hangfire");
#endif
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseCustomSwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.RunBackgroundJobs();
        app.UseHangfireDashboard();

        app.MapControllers();

        await app.RunAsync();
    }
}