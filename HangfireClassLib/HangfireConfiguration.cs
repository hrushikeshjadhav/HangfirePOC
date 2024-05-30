using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;

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

        public void ScheduleRecurringJob(string recType)
        {
            switch (recType.ToLower())
            {
                case "Hourly":
                    RecurringJob.AddOrUpdate("recurring-hourly", () => ExecuteJob(), Cron.Hourly);
                    break;
                case "daily":
                    RecurringJob.AddOrUpdate("recurring-daily", () => ExecuteJob(), "0 17 * * *", TimeZoneInfo.Local);
                    break;
                case "weekly":
                    RecurringJob.AddOrUpdate("recurring-weekly", () => ExecuteJob(), Cron.Weekly);
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
