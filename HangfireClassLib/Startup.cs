using Hangfire;
using Hangfire.PostgreSql;
using HangfireClassLib;
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
        services.AddHangfire(x => x.UsePostgreSqlStorage(connectionString));
    }

    public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs)
    {
        

        // add the retry attempt
        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 1 });

        // Add the custom job filter globally
        GlobalJobFilters.Filters.Add(new TimeoutJobFilter(TimeSpan.FromMinutes(5)));
    }
}
