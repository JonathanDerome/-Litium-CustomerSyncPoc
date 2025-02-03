using Litium.Accelerator.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly CustomerSyncService _customerSyncService;

    public Worker(ILogger<Worker> logger, CustomerSyncService customerSyncService)
    {
        _logger = logger;
        _customerSyncService = customerSyncService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
                await _customerSyncService.SyncCustomersFromFileAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error syncing customers: {ex.Message}");
            }

            Console.WriteLine();
            await Task.Delay(10000, stoppingToken);
        }
    }
}
