using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.ResetPassword
{
    public class ResetPasswordValidator:Validator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x=>x.Request.Password)
                .NotNull().WithMessage("Password Confirmation can not be empty!")
                .NotEmpty().WithMessage("A password is required!")
                .MinimumLength(10).WithMessage("Please provide a longer password")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");

            RuleFor(x => x.Request.ConfirmPassword)
                .NotNull().WithMessage("Password Confirmation can not be empty!")
                .NotEmpty().WithMessage("A password confirmation is required!")
                .Equal(s => s.Request.Password).WithMessage("Password confirmation does not match the provided password.");
        }
    }
}
