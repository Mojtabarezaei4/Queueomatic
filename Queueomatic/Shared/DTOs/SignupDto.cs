namespace Queueomatic.Shared.DTOs;

public class SignupDto
{
    public string Email { get; set; } = null!;
    public string NickName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}