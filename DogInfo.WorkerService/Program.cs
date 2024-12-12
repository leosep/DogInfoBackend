using DogInfo.Jobs;
using DogInfo.Application.Interfaces;
using DogInfo.Infrastructure.Services;
using DogInfo.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using DogInfo.Infrastructure.Repositories;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient<IDogApiService, DogApiService>();

                services.AddDbContext<DogInfoDbContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

                services.AddHangfire(config => config.UseSqlServerStorage(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                services.AddHangfireServer();

                services.AddScoped<BreedDataUpsertJob>();

                using var serviceProvider = services.BuildServiceProvider();
                var jobClient = serviceProvider.GetRequiredService<IBackgroundJobClient>();
                jobClient.Enqueue<BreedDataUpsertJob>(job => job.ExecuteAsync());

                // Run the Worker as a hosted service
                services.AddHostedService<Worker>();
            });
}
