using Microsoft.AspNetCore.Mvc;
using static HangfireConfiguration;

namespace APITestHangfire
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        BackgroundJobs backgroundJobs = new BackgroundJobs();
        public HangfireController() { }
        [HttpPost]
        public async Task<ActionResult> ImmediateAsync(string JobName)
        {
            backgroundJobs.ExecuteJob();

            //var emailService = new EmailService();
            //string toEmail = "recipient@example.com";
            //string subject = "Test Email";
            //string body = "<h1>Hello!</h1><p>This is a test email sent from .NET Core 7.</p>";
            //await emailService.SendEmailAsync(toEmail, subject, body);

            return Ok("success");
        }
        [HttpPost]
        public ActionResult Schedule(DateTime dt)
        {
            backgroundJobs.ScheduleJob(dt);
            return Ok("success");
        }
        [HttpPost]
        public ActionResult Recurring(string recType)
        {
            backgroundJobs.ScheduleRecurringJob(recType);
            return Ok("success");
        }
        [HttpPost]
        public ActionResult AfterJob()
        {
            backgroundJobs.ContinueJob();
            return Ok("success");
        }
        [HttpPost]
        public ActionResult Delete(string JobID)
        {
            backgroundJobs.DeleteJob(JobID);
            return Ok("Job deleted with JobID " + JobID);
        }
    }
}
