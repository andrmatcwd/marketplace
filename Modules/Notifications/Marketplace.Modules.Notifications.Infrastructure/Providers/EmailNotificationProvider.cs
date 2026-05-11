using System.Net;
using System.Net.Mail;
using Marketplace.Modules.Notifications.Application.Notifications.Providers;
using Marketplace.Modules.Notifications.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Marketplace.Modules.Notifications.Infrastructure.Providers;

public sealed class EmailNotificationProvider : INotificationProvider
{
    private readonly EmailNotificationOptions _options;
    private readonly ILogger<EmailNotificationProvider> _logger;

    public EmailNotificationProvider(
        IOptions<ContactNotificationOptions> options,
        ILogger<EmailNotificationProvider> logger)
    {
        _options = options.Value.Email;
        _logger = logger;
    }

    public async Task SendAsync(ContactNotificationPayload payload, CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
            return;

        if (string.IsNullOrWhiteSpace(_options.SmtpHost) ||
            string.IsNullOrWhiteSpace(_options.FromEmail) ||
            string.IsNullOrWhiteSpace(_options.ToEmail))
        {
            _logger.LogWarning("Email provider is enabled but SMTP settings are incomplete.");
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_options.FromEmail, _options.FromName),
            Subject = $"New contact request: {payload.ListingTitle}",
            Body = BuildBody(payload),
            IsBodyHtml = false
        };

        message.To.Add(_options.ToEmail);

        if (!string.IsNullOrWhiteSpace(payload.ListingEmail))
            message.ReplyToList.Add(new MailAddress(payload.ListingEmail));

        using var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
        {
            EnableSsl = _options.UseSsl
        };

        if (!string.IsNullOrWhiteSpace(_options.UserName))
            client.Credentials = new NetworkCredential(_options.UserName, _options.Password);

        await client.SendMailAsync(message, cancellationToken);
    }

    private static string BuildBody(ContactNotificationPayload p)
    {
        return $"""
            New contact request

            Listing:
            {p.ListingTitle}
            ID: {p.ListingId}

            Customer:
            Name: {p.CustomerName}
            Phone: {p.CustomerPhone}

            Message:
            {p.CustomerMessage}

            Listing contacts:
            Phone: {p.ListingPhone}
            Email: {p.ListingEmail}
            Address: {p.ListingAddress}
            """;
    }
}
