namespace DemoApp.Services;

public interface ISmsService
{
    Task<string> SendSmsAsync(string to, string message);
}
