using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static HangfireConfiguration;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("HangfireConnection");

        // Configure Hangfire with the connection string
        services.ConfigureHangfire(connectionString);
    }

    public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs)
    {
        app.UseHangfireDashboard();
        app.UseHangfireServer();

        var jobs = new BackgroundJobs();
        // Schedule a job
        jobs.ScheduleJob(DateTime.Now);

        // Schedule a recurring job
        DateTime dateTime= DateTime.Now;
        jobs.ScheduleRecurringJob("recType", dateTime);

        // Create a continuation job
        jobs.ContinueJob();

        // Delete a job
        jobs.DeleteJob("your-job-id"); // Uncomment and replace with actual job ID to delete

        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 1 });
    }
}
