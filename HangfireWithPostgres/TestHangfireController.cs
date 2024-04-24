using Hangfire;
using Microsoft.AspNetCore.Http;
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
            //BackgroundJob.Enqueue(() => Console.WriteLine("Background job executed."));
            BackgroundJob.Schedule(() => Console.WriteLine("Job scheduled at " + DateTime.Now.AddMinutes(1)), DateTimeOffset.Now.AddMinutes(1));
        }
    }
}
