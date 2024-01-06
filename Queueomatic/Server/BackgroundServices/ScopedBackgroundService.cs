using Queueomatic.Server.Services.RoomDeletionService;

namespace Queueomatic.Server.BackgroundServices;

public sealed class ScopedBackgroundService: BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _timer;
    
    public ScopedBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _timer = new PeriodicTimer(TimeSpan.FromHours(12));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DoWorkAsync(stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken)
               && !stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IRoomDeletionService roomDeletionService =
                scope.ServiceProvider.GetRequiredService<IRoomDeletionService>();

            await roomDeletionService.DeleteExpiredRoomsAsync();
        }
    }
}