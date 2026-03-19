using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoApp.Pages.Chat;

public class IndexModel : PageModel
{
    private readonly IChatService _chatService;
    private readonly IIdentityService _identityService;

    public IndexModel(IChatService chatService, IIdentityService identityService)
    {
        _chatService = chatService;
        _identityService = identityService;
    }

    [BindProperty]
    public new StartChatRequest Request { get; set; } = new();

    public string? ThreadId { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var (userId, token) = await _identityService.CreateUserAndTokenAsync();
        ThreadId = await _chatService.CreateThreadAsync(Request.Topic, token, userId);
        return Page();
    }
}
