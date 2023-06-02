using System.ComponentModel.DataAnnotations;

namespace Queueomatic.Shared.DTOs;

public class ResetPassword
{
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}