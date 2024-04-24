using Hangfire;

namespace HangfireWithPostgres
{
    public class HangfireTest
    {
        public void Test()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Background job executed."));
        }
    }
}
