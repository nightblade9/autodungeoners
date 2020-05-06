using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AutoDungeoners.Web.Services
{
    /// <summary>
    /// A base class for services, it provides a safe way to run/cancel every second, and exposes
    /// a method to implement service-specific logic based on the amount of time elapsed.
    /// </summary>
    // Mostly copied from https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=netcore-cli#timed-background-tasks
    abstract class AbstractService : IAbstractService, IHostedService
    {
        public abstract Task OnTick(TimeSpan elapsedTime);
        
        private DateTime lastRunTime = DateTime.Now;
        private bool isRunning;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.isRunning = !cancellationToken.IsCancellationRequested;

            while (this.isRunning && !cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var elapsed = now - this.lastRunTime;
                if (elapsed.TotalSeconds >= 1)
                {
                    this.lastRunTime = now;
                    this.OnTick(elapsed);
                }
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.isRunning = false;
            return Task.CompletedTask;
        }
    }
}