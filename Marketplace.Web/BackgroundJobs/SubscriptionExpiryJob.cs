using Marketplace.Modules.Listings.Application.Subscriptions.Commands.ExpireSubscriptions;
using MediatR;

namespace Marketplace.Web.BackgroundJobs;

public class SubscriptionExpiryJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SubscriptionExpiryJob> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    public SubscriptionExpiryJob(IServiceScopeFactory scopeFactory, ILogger<SubscriptionExpiryJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(new ExpireSubscriptionsCommand(), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error expiring subscriptions.");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }
}
