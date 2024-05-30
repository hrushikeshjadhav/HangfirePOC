using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangfireWithPostgres
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestHangfireController : ControllerBase
    {
        [HttpGet]
        public void CallJob()
        {
            //BackgroundJob.Enqueue(() => Console.WriteLine("Background job Enqueued."));
            //BackgroundJob.Schedule(() => Console.WriteLine("Job scheduled at " + DateTime.Now.AddMinutes(2)), DateTimeOffset.Now.AddMinutes(1));
            RecurringJob.AddOrUpdate("jobId", () => Console.WriteLine("Recurring JOb scheduled after every minute"), Cron.Minutely);
        }
    }
}
