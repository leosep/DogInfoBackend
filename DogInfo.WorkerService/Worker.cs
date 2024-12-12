using DogInfo.Jobs;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRecurringJobManager _recurringJobManager;

    public Worker(ILogger<Worker> logger, IRecurringJobManager recurringJobManager)
    {
        _logger = logger;
        _recurringJobManager = recurringJobManager;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running and scheduling Hangfire jobs.");

        // Schedule the BreedDataUpsertJob to run every hour
        _recurringJobManager.AddOrUpdate<BreedDataUpsertJob>(
            "BreedDataUpsertJob",
            job => job.ExecuteAsync(),
            Cron.Hourly);

        _logger.LogInformation("Hangfire job scheduled: BreedDataUpsertJob (runs every hour).");

        return Task.CompletedTask;
    }
}
