using Azure.Communication.Email;

namespace DemoApp.Services;

public class EmailService : IEmailService
{
    private readonly EmailClient? _client;
    private readonly bool _isConfigured;

    public EmailService(string connectionString)
    {
        _isConfigured = !string.IsNullOrWhiteSpace(connectionString);
        if (_isConfigured)
            _client = new EmailClient(connectionString);
    }

    public async Task<string> SendEmailAsync(string to, string subject, string body, string? from = null)
    {
        if (!_isConfigured || _client == null)
            return "demo-message-id-" + Guid.NewGuid().ToString("N")[..8];

        var senderAddress = from ?? Environment.GetEnvironmentVariable("ACS_EMAIL_SENDER") ?? "donotreply@azurecomm.net";
        var emailMessage = new EmailMessage(
            senderAddress: senderAddress,
            content: new EmailContent(subject) { Html = body },
            recipients: new EmailRecipients(new[] { new EmailAddress(to) }));

        var operation = await _client.SendAsync(Azure.WaitUntil.Completed, emailMessage);
        return operation.Id ?? Guid.NewGuid().ToString();
    }

    public async Task<List<string>> SendBulkEmailAsync(List<string> recipients, string subject, string body, string? from = null)
    {
        if (!_isConfigured || _client == null)
            return recipients.Select(r => "demo-id-" + Guid.NewGuid().ToString("N")[..8]).ToList();

        var senderAddress = from ?? Environment.GetEnvironmentVariable("ACS_EMAIL_SENDER") ?? "donotreply@azurecomm.net";
        var emailAddresses = recipients.Select(r => new EmailAddress(r)).ToList();
        var emailMessage = new EmailMessage(
            senderAddress: senderAddress,
            content: new EmailContent(subject) { Html = body },
            recipients: new EmailRecipients(emailAddresses));

        var operation = await _client.SendAsync(Azure.WaitUntil.Completed, emailMessage);
        return [operation.Id ?? Guid.NewGuid().ToString()];
    }
}
