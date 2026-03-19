using Azure.Communication.Sms;

namespace DemoApp.Services;

public class SmsService : ISmsService
{
    private readonly SmsClient? _client;
    private readonly string _fromNumber;
    private readonly bool _isConfigured;

    public SmsService(string connectionString, string fromNumber)
    {
        _fromNumber = fromNumber;
        _isConfigured = !string.IsNullOrWhiteSpace(connectionString) && !string.IsNullOrWhiteSpace(fromNumber);
        if (!string.IsNullOrWhiteSpace(connectionString))
            _client = new SmsClient(connectionString);
    }

    public async Task<string> SendSmsAsync(string to, string message)
    {
        if (!_isConfigured || _client == null)
            return "demo-sms-id-" + Guid.NewGuid().ToString("N")[..8];

        var response = await _client.SendAsync(from: _fromNumber, to: to, message: message);
        return response.Value.MessageId ?? "unknown";
    }
}
