using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoApp.Pages;

public class IndexModel : PageModel
{
    public bool IsConfigured { get; private set; }

    public void OnGet()
    {
        IsConfigured = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ACS_CONNECTION_STRING"));
    }
}
