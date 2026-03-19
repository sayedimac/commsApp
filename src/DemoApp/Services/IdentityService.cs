using Azure.Communication;
using Azure.Communication.Identity;

namespace DemoApp.Services;

public class IdentityService : IIdentityService
{
    private readonly CommunicationIdentityClient? _client;
    private readonly bool _isConfigured;

    public IdentityService(string connectionString)
    {
        _isConfigured = !string.IsNullOrWhiteSpace(connectionString);
        if (_isConfigured)
        {
            _client = new CommunicationIdentityClient(connectionString);
        }
    }

    public async Task<(string UserId, string Token)> CreateUserAndTokenAsync()
    {
        if (!_isConfigured || _client == null)
        {
            var demoId = "demo-user-" + Guid.NewGuid().ToString("N")[..8];
            var demoToken = "demo-token-" + Guid.NewGuid().ToString("N")[..8];
            return (demoId, demoToken);
        }

        var response = await _client.CreateUserAndTokenAsync(
            scopes: [CommunicationTokenScope.Chat, CommunicationTokenScope.VoIP]);
        return (response.Value.User.Id, response.Value.AccessToken.Token);
    }

    public async Task<string> GetTokenForUserAsync(string userId)
    {
        if (!_isConfigured || _client == null)
            return "demo-token-" + Guid.NewGuid().ToString("N")[..8];

        var identifier = new CommunicationUserIdentifier(userId);
        var response = await _client.GetTokenAsync(identifier,
            scopes: [CommunicationTokenScope.Chat, CommunicationTokenScope.VoIP]);
        return response.Value.Token;
    }

    public async Task DeleteUserAsync(string userId)
    {
        if (!_isConfigured || _client == null)
            return;

        var identifier = new CommunicationUserIdentifier(userId);
        await _client.DeleteUserAsync(identifier);
    }
}
