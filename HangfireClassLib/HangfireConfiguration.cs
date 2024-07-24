using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;

public static class HangfireConfiguration
{
    public static void ConfigureHangfire(this IServiceCollection services, string connectionString)
    {
        services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(connectionString);
        });

        services.AddHangfireServer();
    }

    public class BackgroundJobs
    {
        public void ExecuteJob()
        {
            BackgroundJob.Enqueue(() => ExecuteJob());
        }

        public void ScheduleJob(DateTime dt)
        {
            BackgroundJob.Schedule("schedulejob", () => ExecuteJob(), dt);
        }

        public void ScheduleRecurringJob(string jobName, string recType, DateTime date, TimeOnly time)
        {
            DateTime scheduleTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second); // Example: June 26, 2024, at 12:00 PM
            string cronExpression = $"{scheduleTime.Minute} {scheduleTime.Hour} * * *";
            switch (recType.ToLower())
            {
                case "hourly":
                    RecurringJob.AddOrUpdate(jobName, () => ExecuteJob(), Cron.Hourly);
                    break;
                case "daily":
                    RecurringJob.AddOrUpdate(jobName, () => ExecuteJob(), cronExpression, TimeZoneInfo.Local);
                    break;
                case "weekly":
                    RecurringJob.AddOrUpdate(jobName, () => ExecuteJob(), Cron.Weekly());
                    break;
            }
        }

        public void ContinueJob()
        {
            var parentJobId = BackgroundJob.Enqueue(() => ExecuteJob());
            BackgroundJob.ContinueWith(parentJobId, () => ExecuteJob());
        }

        public void DeleteJob(string jobId)
        {
            BackgroundJob.Delete(jobId);
        }
    }
}
