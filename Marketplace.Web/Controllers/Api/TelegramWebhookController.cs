using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("api/telegram")]
public sealed class TelegramWebhookController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _botToken;
    private readonly ILogger<TelegramWebhookController> _logger;

    public TelegramWebhookController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<TelegramWebhookController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _botToken = configuration["Telegram:BotToken"]!;
        _logger = logger;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook(
        [FromBody] TelegramUpdateDto update,
        CancellationToken cancellationToken)
    {
        if (update?.Message == null)
            return Ok();

        var message = update.Message;
        var chat = message.Chat;

        if (chat == null)
            return Ok();

        try
        {
            // 🔹 знайти або створити користувача
            // var recipient = await _dbContext.TelegramRecipients
            //     .FirstOrDefaultAsync(x => x.ChatId == chat.Id, cancellationToken);

            // if (recipient == null)
            // {
            //     recipient = new TelegramRecipient
            //     {
            //         ChatId = chat.Id,
            //         CreatedAtUtc = DateTime.UtcNow
            //     };

            //     _dbContext.TelegramRecipients.Add(recipient);
            // }

            // // 🔹 базові дані
            // recipient.Username = message.From?.Username ?? chat.Username;
            // recipient.FirstName = message.From?.FirstName ?? chat.FirstName;
            // recipient.LastName = message.From?.LastName ?? chat.LastName;
            // recipient.TelegramUserId = message.From?.Id;
            // recipient.LastSeenAtUtc = DateTime.UtcNow;
            // recipient.IsActive = true;

            // // 🔥 1. Якщо /start → просимо телефон
            // if (!string.IsNullOrWhiteSpace(message.Text) &&
            //     message.Text.StartsWith("/start"))
            // {
            //     await SendPhoneRequestAsync(chat.Id, cancellationToken);
            // }

            // // 🔥 2. Якщо прийшов контакт → зберігаємо
            // if (message.Contact != null)
            // {
            //     if (message.Contact.UserId == message.From?.Id)
            //     {
            //         recipient.PhoneNumber = message.Contact.PhoneNumber;

            //         await SendSimpleMessageAsync(
            //             chat.Id,
            //             "Дякуємо! Номер успішно збережено ✅",
            //             cancellationToken);
            //     }
            // }

            // await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Telegram webhook error");
            return Ok();
        }
    }

    // 🔹 КНОПКА ЗАПИТУ ТЕЛЕФОНУ
    private async Task SendPhoneRequestAsync(
        long chatId,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();

        var payload = new
        {
            chat_id = chatId,
            text = "Щоб отримувати заявки — поділіться номером телефону",
            reply_markup = new
            {
                keyboard = new[]
                {
                    new[]
                    {
                        new
                        {
                            text = "Поділитися номером",
                            request_contact = true
                        }
                    }
                },
                resize_keyboard = true,
                one_time_keyboard = true
            }
        };

        await client.PostAsJsonAsync(
            $"https://api.telegram.org/bot{_botToken}/sendMessage",
            payload,
            cancellationToken);
    }

    // 🔹 ПРОСТЕ ПОВІДОМЛЕННЯ
    private async Task SendSimpleMessageAsync(
        long chatId,
        string text,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();

        var payload = new
        {
            chat_id = chatId,
            text = text
        };

        await client.PostAsJsonAsync(
            $"https://api.telegram.org/bot{_botToken}/sendMessage",
            payload,
            cancellationToken);
    }
}

public sealed class TelegramUpdateDto
{
    [JsonPropertyName("message")]
    public TelegramMessageDto? Message { get; set; }
}

public sealed class TelegramMessageDto
{
    [JsonPropertyName("chat")]
    public TelegramChatDto? Chat { get; set; }

    [JsonPropertyName("from")]
    public TelegramUserDto? From { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("contact")]
    public TelegramContactDto? Contact { get; set; }
}

public sealed class TelegramChatDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }
}

public sealed class TelegramUserDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }
}

public sealed class TelegramContactDto
{
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("user_id")]
    public long? UserId { get; set; }
}