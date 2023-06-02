using System.ComponentModel.DataAnnotations;

namespace Queueomatic.Shared.DTOs;

public class ResetPasswordDto
{
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}