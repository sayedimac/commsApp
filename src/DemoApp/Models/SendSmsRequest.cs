using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models;

public class SendSmsRequest
{
    [Required]
    [Phone]
    public string To { get; set; } = string.Empty;

    [Required]
    [StringLength(1600)]
    public string Message { get; set; } = string.Empty;
}
