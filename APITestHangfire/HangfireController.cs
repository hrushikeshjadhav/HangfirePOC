using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using static Hangfire.Storage.JobStorageFeatures;
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
            var options = new BackgroundJobServerOptions
            {
                WorkerCount = 2 // Set the number of worker threads to 2.
            };

            using (var server = new BackgroundJobServer(options))
            {
                // Schedule two background jobs.
                BackgroundJob.Enqueue(() => Job1());
                BackgroundJob.Enqueue(() => Job2());

                Console.WriteLine("Jobs have been scheduled. Press any key to exit...");
                Console.ReadKey();
            }
            return Ok("success");
        }

        public static void Job1()
        {
            Console.WriteLine("Job1 started.");
            // Email code commented below
            //var emailService = new EmailService();
            //string toEmail = "recipient@example.com";
            //string subject = "Test Email";
            //string body = "<h1>Hello!</h1><p>This is a test email sent from .NET Core 7.</p>";
            //await emailService.SendEmailAsync(toEmail, subject, body);
            Task.Delay(5000).Wait(); // Simulate a long-running task.
            Console.WriteLine("Email sent & Job1 completed.");
        }

        public static void Job2()
        {
            Console.WriteLine("Job2 started.");
            string taskList = "Assign task 1" + "Assign task 2";
            Task.Delay(5000).Wait(); // Simulate a long-running task.
            Console.WriteLine(taskList + " created & Job2 completed.");
        }

        [HttpPost]
        public ActionResult Schedule(DateTime dt)
        {
            backgroundJobs.ScheduleJob(dt);
            return Ok("success");
        }

        [HttpPost]
        public ActionResult Recurring(string jobName, string recType, DateTime dateTime, TimeOnly time)
        {
            backgroundJobs.ScheduleRecurringJob(jobName, recType, dateTime, time);
            return Ok("success");
        }

        [HttpPost]
        public ActionResult AfterJob(string parentJobId)
        {
            backgroundJobs.ContinueJob(parentJobId);
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
