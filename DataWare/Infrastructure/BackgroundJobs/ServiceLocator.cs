using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure.BackgroundJobs;

internal static class ServiceLocator
{
    public static IServiceCollection ConfigureBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var launchFlightSearchJobKey = new JobKey(nameof(LaunchFlightSearchJob));

            configure.AddJob<LaunchFlightSearchJob>(launchFlightSearchJobKey)
                .AddTrigger(trigger =>
                {
                    trigger.ForJob(launchFlightSearchJobKey)
                        .WithSimpleSchedule(schedule =>
                        {
                            schedule.WithIntervalInSeconds(3)
                                .RepeatForever();
                        });
                });
        });

        services.AddQuartzHostedService();

        return services;
    }
}
