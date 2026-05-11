namespace Marketplace.Web.Options;

public sealed class EmailNotificationOptions
{
    public bool Enabled { get; set; }

    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public bool UseSsl { get; set; } = true;

    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "Marketplace";
    public string ToEmail { get; set; } = string.Empty;
}