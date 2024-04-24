using Hangfire.Dashboard;
using Hangfire;
using Hangfire.PostgreSql;

namespace HangfireWithPostgres
{
    public class Startup
    {
        private readonly string connectionString = "HOST=zing2-dev-db.postgres.database.azure.com;DATABASE=POSTGRES;PORT=5432;USER=postgres_dev_user;Password=ZingHR#dev@2024;";
        public void ConfigureServices(IServiceCollection services)
        {
            //Add Hangfire services
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(connectionString));


            // Add Hangfire server
            services.AddHangfireServer();



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerName = Environment.GetEnvironmentVariable("HangfireSever") ?? "India",
                WorkerCount = 6,
                SchedulePollingInterval = TimeSpan.FromSeconds(30),
                Queues = new string[] { Environment.GetEnvironmentVariable("HangfireSever")?.ToLower() ?? "india", "default" }
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
        }

        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // Implement your authorization logic here
                // For example, you can check if the user is authenticated
                return true; // Allow all users for simplicity (customize this according to your needs)
            }
        }

    }
}
