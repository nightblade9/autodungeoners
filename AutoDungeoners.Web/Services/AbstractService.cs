using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoDungeoners.Web.Services
{
    /// <summary>
    /// A base class for services, it provides a safe way to run/cancel every second, and exposes
    /// a method to implement service-specific logic based on the amount of time elapsed.
    /// </summary>
    abstract class AbstractService : IAbstractService
    {
        public abstract Task OnTick(TimeSpan elapsedTime);
        
        private DateTime lastRunTime;

        // Mostly copied from https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=netcore-cli#timed-background-tasks
        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var elapsed = now - this.lastRunTime;
                if (elapsed.TotalSeconds >= 1)
                {
                    this.lastRunTime = now;
                    await this.OnTick(elapsed);
                }
            }
        }
    }
}