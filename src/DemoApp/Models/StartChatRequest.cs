using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models;

public class StartChatRequest
{
    [Required]
    public string Topic { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string ParticipantEmail { get; set; } = string.Empty;
}
