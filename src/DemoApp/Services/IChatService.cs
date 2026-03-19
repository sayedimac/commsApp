namespace DemoApp.Services;

public interface IChatService
{
    Task<string> CreateThreadAsync(string topic, string userToken, string userId);
    Task SendMessageAsync(string threadId, string userToken, string message, string displayName);
    Task<List<string>> GetMessagesAsync(string threadId, string userToken);
}
