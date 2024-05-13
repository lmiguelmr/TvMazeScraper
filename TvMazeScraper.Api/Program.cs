using TvMazeScraper.Infrastructure.Extensions;
using TvMazeScraper.Api.Extensions;
using TvMazeScraper.Application;
using TvMazeScraper.Api.OpenApi;
using Hangfire;
using TvMazeScraper.Infrastructure.TvMazeIntegration;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

IWebHostEnvironment? hostEnvironment = null;

try
{
    Log.Information("Initializing.");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    hostEnvironment = builder.Environment;

    IConfiguration configuration = builder.Configuration;

    builder.Services.AddControllers();
    builder.Services.AddProblemDetails();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddApplication();
    builder.Services.AddServices(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

    builder.Services.AddHttpClient("TvMazeApiClient",
        client =>
        {
            client.BaseAddress = new Uri(configuration["TvMazeApiClientSettings:BasePath"] ?? "");
        })
        .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder
        .OrResult(x => x.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(10), 3)));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();

            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });

        app.ApplyMigrations();
    }

    app.UseRouting();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseHangfireDashboard();
    app.UseHangfireServer();

    RecurringJob.AddOrUpdate<TvMazeJobs>($"{typeof(TvMazeJobs).Name}.ImportAll()", x => x.ImportAll(null!), Cron.Never);
    RecurringJob.AddOrUpdate<TvMazeJobs>($"{typeof(TvMazeJobs).Name}.Update()", x => x.Update(null!), Cron.Daily);

    Log.Information("Started {Application} in {Environment} mode",
        hostEnvironment.ApplicationName, hostEnvironment.EnvironmentName);

    await app.RunAsync();

    Log.Information("Stopped {Application} in {Environment} mode",
        hostEnvironment.ApplicationName, hostEnvironment.EnvironmentName);

    return 0;
}
catch (Exception exception)
{
    Log.Fatal(exception,
        "{Application} terminated unexpectedly in {Environment} mode.",
    hostEnvironment?.ApplicationName, hostEnvironment?.EnvironmentName);

    return 1;
}
finally
{
    Log.CloseAndFlush();
    // Ensures that log messages are sent to Application Insights when application abruptly finishes
    await Task.Delay(TimeSpan.FromSeconds(1));
}