using System.Transactions;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerService3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped(x => new TransactionScope());
                    services.AddScoped(x => new MyBackgroundJob(x.GetRequiredService<TransactionScope>()));

                    services.AddHangfire((provider, config) => config
                        .UseFilter(new TransactionScopeServerFilter(provider.GetRequiredService<IServiceScopeFactory>()))
                        .UseMemoryStorage());

                    services.AddHangfireServer();
                    services.AddHostedService<WorkerService>();
                });
    }
}
