

using DemoWorker.Services;

namespace DemoWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer periodicTimer = new(TimeSpan.FromSeconds(3));
            while (!stoppingToken.IsCancellationRequested && await periodicTimer.WaitForNextTickAsync())
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using var scope = _scopeFactory.CreateScope();
                {
                    var service = scope.ServiceProvider.GetRequiredService<PublishEvent>();

                    await service.DoWork(stoppingToken);
                };
            }
        }
    }
}
