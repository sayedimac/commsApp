namespace DemoApp.Services;

public interface IIdentityService
{
    Task<(string UserId, string Token)> CreateUserAndTokenAsync();
    Task<string> GetTokenForUserAsync(string userId);
    Task DeleteUserAsync(string userId);
}
