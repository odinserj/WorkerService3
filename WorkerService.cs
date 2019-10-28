using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace WorkerService3
{
    public class WorkerService : IHostedService
    {
        private readonly IBackgroundJobClient _backgroundJobs;

        public WorkerService(IBackgroundJobClient backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _backgroundJobs.Enqueue<MyBackgroundJob>(x => x.MyMethod());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}