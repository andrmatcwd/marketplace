using System.Net;
using System.Text;
using System.Text.Json;
using Marketplace.Modules.Notifications.Application.Notifications.Providers;
using Marketplace.Modules.Notifications.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Marketplace.Modules.Notifications.Infrastructure.Providers;

public sealed class TelegramNotificationProvider : INotificationProvider
{
    private readonly TelegramNotificationOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TelegramNotificationProvider> _logger;

    public TelegramNotificationProvider(
        IOptions<ContactNotificationOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<TelegramNotificationProvider> logger)
    {
        _options = options.Value.Telegram;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task SendAsync(ContactNotificationPayload payload, CancellationToken cancellationToken)
    {
        if (!_options.Enabled)
            return;

        if (string.IsNullOrWhiteSpace(_options.BotToken) || string.IsNullOrWhiteSpace(_options.ChatId))
        {
            _logger.LogWarning("Telegram provider is enabled but BotToken or ChatId is missing.");
            return;
        }

        var message = BuildMessage(payload);

        var body = new
        {
            chat_id = _options.ChatId,
            text = message,
            parse_mode = "HTML",
            disable_web_page_preview = true
        };

        var client = _httpClientFactory.CreateClient();
        var url = $"https://api.telegram.org/bot{_options.BotToken}/sendMessage";

        using var content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        using var response = await client.PostAsync(url, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning(
                "Telegram notification failed. Status: {StatusCode}. Body: {Body}",
                response.StatusCode, responseBody);
        }
    }

    private static string BuildMessage(ContactNotificationPayload p)
    {
        return $"""
            <b>Нова заявка з Marketplace</b>

            <b>Послуга:</b> {Escape(p.ListingTitle)}
            <b>Listing ID:</b> <code>{p.ListingId}</code>

            <b>Клієнт:</b> {Escape(p.CustomerName)}
            <b>Телефон:</b> {Escape(p.CustomerPhone)}

            <b>Повідомлення:</b>
            {Escape(p.CustomerMessage)}

            <b>Контакти listing:</b>
            {Escape(p.ListingPhone ?? "—")}
            {Escape(p.ListingEmail ?? "—")}
            {Escape(p.ListingAddress ?? "—")}
            """;
    }

    private static string Escape(string value) => WebUtility.HtmlEncode(value);
}
