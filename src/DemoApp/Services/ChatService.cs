using Azure.Communication;
using Azure.Communication.Chat;

namespace DemoApp.Services;

public class ChatService : IChatService
{
    private readonly string _connectionString;
    private readonly bool _isConfigured;

    public ChatService(string connectionString)
    {
        _connectionString = connectionString;
        _isConfigured = !string.IsNullOrWhiteSpace(connectionString);
    }

    public async Task<string> CreateThreadAsync(string topic, string userToken, string userId)
    {
        if (!_isConfigured)
            return "demo-thread-" + Guid.NewGuid().ToString("N")[..8];

        var endpoint = ExtractEndpoint(_connectionString);
        var chatClient = new ChatClient(new Uri(endpoint), new Azure.Communication.CommunicationTokenCredential(userToken));

        var participant = new ChatParticipant(new CommunicationUserIdentifier(userId))
        {
            DisplayName = "Demo User"
        };

        var response = await chatClient.CreateChatThreadAsync(topic, new[] { participant });
        return response.Value.ChatThread.Id;
    }

    public async Task SendMessageAsync(string threadId, string userToken, string message, string displayName)
    {
        if (!_isConfigured)
            return;

        var endpoint = ExtractEndpoint(_connectionString);
        var chatClient = new ChatClient(new Uri(endpoint), new Azure.Communication.CommunicationTokenCredential(userToken));
        var threadClient = chatClient.GetChatThreadClient(threadId);
        await threadClient.SendMessageAsync(message, Azure.Communication.Chat.ChatMessageType.Text, displayName);
    }

    public async Task<List<string>> GetMessagesAsync(string threadId, string userToken)
    {
        if (!_isConfigured)
            return ["[Demo] Hello! How can I help you today?", "[Demo] This is a simulated chat message."];

        var endpoint = ExtractEndpoint(_connectionString);
        var chatClient = new ChatClient(new Uri(endpoint), new Azure.Communication.CommunicationTokenCredential(userToken));
        var threadClient = chatClient.GetChatThreadClient(threadId);

        var messages = new List<string>();
        await foreach (var msg in threadClient.GetMessagesAsync())
        {
            if (msg.Type == ChatMessageType.Text)
                messages.Add($"[{msg.SenderDisplayName}] {msg.Content?.Message}");
        }
        return messages;
    }

    private static string ExtractEndpoint(string connectionString)
    {
        var parts = connectionString.Split(';');
        foreach (var part in parts)
        {
            if (part.StartsWith("endpoint=", StringComparison.OrdinalIgnoreCase))
                return part[9..];
        }
        return string.Empty;
    }
}
