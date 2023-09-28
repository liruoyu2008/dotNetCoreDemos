using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServerDemo
{
    /// <summary>
    /// 生命周期事件托管
    /// </summary>
    internal class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            //可以启动一个后台任务
            Console.WriteLine("OnStarted has been called.");
            _logger.LogInformation("OnStarted has been called.");
        }

        private void OnStopping()
        {
            Console.WriteLine("OnStopping has been called.");
            _logger.LogInformation("OnStopping has been called.");
        }

        private void OnStopped()
        {
            Console.WriteLine("OnStopped has been called.");
            _logger.LogInformation("OnStopped has been called.");
        }
    }

}
