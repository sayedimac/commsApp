namespace DemoApp.Services;

public interface IEmailService
{
    Task<string> SendEmailAsync(string to, string subject, string body, string? from = null);
    Task<List<string>> SendBulkEmailAsync(List<string> recipients, string subject, string body, string? from = null);
}
