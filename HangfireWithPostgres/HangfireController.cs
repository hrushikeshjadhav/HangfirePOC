using Microsoft.AspNetCore.Mvc;
using HangfireWithPostgres;
using SendMail;

namespace APITestHangfire
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        HangFireEvents fireEvents = new HangFireEvents();
        public HangfireController() { }
        [HttpGet]
        public async Task<ActionResult> ImmediateAsync(string JobName)
        {
            fireEvents.ImmediateAsync(JobName);

            //var emailService = new EmailService();
            //string toEmail = "recipient@example.com";
            //string subject = "Test Email";
            //string body = "<h1>Hello!</h1><p>This is a test email sent from .NET Core 7.</p>";
            //await emailService.SendEmailAsync(toEmail, subject, body);

            return Ok("success");
        }
        [HttpPost]
        public ActionResult Schedule(string JobName, DateTime dt)
        {
            fireEvents.Schedule(JobName, dt);
            return Ok("success");
        }
        [HttpPost]
        public ActionResult Recurring(string JobName)
        {
            fireEvents.Recurring(JobName);
            return Ok("success");
        }
        [HttpPost]
        public ActionResult AfterJob(string JobName, string JobID)
        {
            fireEvents.AfterJob(JobName, JobID);
            return Ok("success");
        }
        [HttpPost]
        public ActionResult Delete(string JobName, string JobID)
        {
            fireEvents.Delete(JobName, JobID);
            return Ok("Job deleted with JobID " + JobID);
        }
    }
}
