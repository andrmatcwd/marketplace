using Marketplace.Modules.Notifications.Application.Notifications.Providers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.SendContactNotification;

public sealed class SendContactNotificationHandler : IRequestHandler<SendContactNotificationCommand, Unit>
{
    private readonly IEnumerable<INotificationProvider> _providers;
    private readonly ILogger<SendContactNotificationHandler> _logger;

    public SendContactNotificationHandler(
        IEnumerable<INotificationProvider> providers,
        ILogger<SendContactNotificationHandler> logger)
    {
        _providers = providers;
        _logger = logger;
    }

    public async Task<Unit> Handle(SendContactNotificationCommand request, CancellationToken cancellationToken)
    {
        var payload = new ContactNotificationPayload
        {
            ListingId = request.ListingId,
            ListingTitle = request.ListingTitle,
            ListingPhone = request.ListingPhone,
            ListingEmail = request.ListingEmail,
            ListingAddress = request.ListingAddress,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            CustomerMessage = request.CustomerMessage
        };

        var tasks = _providers.Select(p => p.SendAsync(payload, cancellationToken));

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "One or more notification providers failed for listing {ListingId}.", request.ListingId);
        }

        return Unit.Value;
    }
}
