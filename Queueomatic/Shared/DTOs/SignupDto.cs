namespace Queueomatic.Shared.DTOs;

public class SignupDto
{
    public string Email { get; set; }
    public string? NickName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}