﻿using Hangfire;

public class BackgroundJobs
{
    public string ImmediateAsync(string JobName)
    {
        BackgroundJob.Enqueue(() => Console.WriteLine(JobName + " job Enqueued."));
        return "success";
    }

    public string Schedule(DateTime  dt)
    {
        BackgroundJob.Schedule(() => Console.WriteLine(" job scheduled at " + dt), dt);
        return "success";
    }
    public string Recurring(string JobName)
    {
        RecurringJob.AddOrUpdate("jobId", () => Console.WriteLine("Recurring JOb " + JobName + " scheduled after every minute"), Cron.Minutely);
        return "success";
    }
    public string AfterJob(string JobName, string JobID)
    {
        BackgroundJob.ContinueJobWith(JobID, () => Console.WriteLine(JobName + " job Enqueued."));
        return "success";
    }
    public string Delete(string JobName, string JobID)
    {
        BackgroundJob.Delete(JobID);
        return JobID + " deleted successfully";
    }
}
