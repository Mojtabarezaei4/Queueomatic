using System.ComponentModel.DataAnnotations;

namespace Queueomatic.Shared.DTOs;

public class SignupDto
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [DataType(DataType.Text)]
    public string? NickName { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, ErrorMessage = "Password must be 10 characters at minimum", MinimumLength = 10)]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$", ErrorMessage = "Your password must contain at least one uppercase letter, one lowercase letter, one number, one (!? *.)")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Confirm Password is required")]
    [StringLength(255, ErrorMessage = "Confirm Password must be 10 characters at minimum", MinimumLength = 10)]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}