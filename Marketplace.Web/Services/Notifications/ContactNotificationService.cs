using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Web.Models.Api;
using Marketplace.Web.Options;
using Microsoft.Extensions.Options;

namespace Marketplace.Web.Services.Notifications;

public sealed class ContactNotificationService : IContactNotificationService
{
    private readonly ContactNotificationOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ContactNotificationService> _logger;

    public ContactNotificationService(
        IOptions<ContactNotificationOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<ContactNotificationService> logger)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task NotifyContactRequestAsync(
        ListingDetailsDto listing,
        ListingContactRequestDto request,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        if (_options.Email.Enabled)
        {
            tasks.Add(SendEmailAsync(listing, request, cancellationToken));
        }

        if (_options.Telegram.Enabled)
        {
            tasks.Add(SendTelegramAsync(listing, request, cancellationToken));
        }

        if (tasks.Count == 0)
        {
            return;
        }

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send one or more contact request notifications.");
        }
    }

    private async Task SendEmailAsync(
        ListingDetailsDto listing,
        ListingContactRequestDto request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.Email.SmtpHost) ||
            string.IsNullOrWhiteSpace(_options.Email.FromEmail) ||
            string.IsNullOrWhiteSpace(_options.Email.ToEmail))
        {
            _logger.LogWarning("Email notification is enabled but SMTP settings are incomplete.");
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_options.Email.FromEmail, _options.Email.FromName),
            Subject = $"New contact request: {listing.Title}",
            Body = BuildEmailBody(listing, request),
            IsBodyHtml = false
        };

        message.To.Add(_options.Email.ToEmail);

        if (!string.IsNullOrWhiteSpace(listing.Email))
        {
            message.ReplyToList.Add(new MailAddress(listing.Email));
        }

        using var client = new SmtpClient(_options.Email.SmtpHost, _options.Email.SmtpPort)
        {
            EnableSsl = _options.Email.UseSsl
        };

        if (!string.IsNullOrWhiteSpace(_options.Email.UserName))
        {
            client.Credentials = new NetworkCredential(
                _options.Email.UserName,
                _options.Email.Password);
        }

        await client.SendMailAsync(message, cancellationToken);
    }

    private async Task SendTelegramAsync(
        ListingDetailsDto listing,
        ListingContactRequestDto request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.Telegram.BotToken) ||
            string.IsNullOrWhiteSpace(_options.Telegram.ChatId))
        {
            _logger.LogWarning("Telegram notification is enabled but settings are incomplete.");
            return;
        }

        var text = BuildTelegramText(listing, request);

        var payload = new
        {
            chat_id = _options.Telegram.ChatId,
            text,
            parse_mode = "HTML",
            disable_web_page_preview = true
        };

        var client = _httpClientFactory.CreateClient();

        var url = $"https://api.telegram.org/bot{_options.Telegram.BotToken}/sendMessage";

        using var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        using var response = await client.PostAsync(url, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogWarning(
                "Telegram notification failed. Status: {StatusCode}. Body: {Body}",
                response.StatusCode,
                responseBody);
        }
    }

    private static string BuildEmailBody(ListingDetailsDto listing, ListingContactRequestDto request)
    {
        return $"""
        New contact request

        Listing:
        {listing.Title}
        ID: {listing.Id}

        Customer:
        Name: {request.Name}
        Phone: {request.Phone}

        Message:
        {request.Message}

        Listing contacts:
        Phone: {listing.Phone}
        Email: {listing.Email}
        Address: {listing.Address}
        """;
    }

    private static string BuildTelegramText(ListingDetailsDto listing, ListingContactRequestDto request)
    {
        return $"""
        <b>Нова заявка з Marketplace</b>

        <b>Послуга:</b> {Escape(listing.Title)}
        <b>Listing ID:</b> <code>{listing.Id}</code>

        <b>Клієнт:</b> {Escape(request.Name)}
        <b>Телефон:</b> {Escape(request.Phone)}

        <b>Повідомлення:</b>
        {Escape(request.Message)}

        <b>Контакти listing:</b>
        {Escape(listing.Phone ?? "—")}
        {Escape(listing.Email ?? "—")}
        {Escape(listing.Address ?? "—")}
        """;
    }

    private static string Escape(string value)
    {
        return WebUtility.HtmlEncode(value);
    }
}