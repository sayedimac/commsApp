using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models;

public class SendBulkEmailRequest
{
    [Required]
    public List<string> Recipients { get; set; } = new();

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    public string RecipientsRaw { get; set; } = string.Empty;
}
