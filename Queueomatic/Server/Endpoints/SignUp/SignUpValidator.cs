using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.SignUp;

public class SignUpValidator : Validator<SignUpRequest>
{
    public SignUpValidator()
    {
        RuleFor(x => x.Signup.Password)
            .NotNull().WithMessage("Password Confirmation can not be empty!")
            .NotEmpty().WithMessage("A password is required!")
            .MinimumLength(10).WithMessage("Please provide a longer password")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");

        RuleFor(x => x.Signup.ConfirmPassword)
            .NotNull().WithMessage("Password Confirmation can not be empty!")
            .NotEmpty().WithMessage("A password confirmation is required!")
            .Equal(s => s.Signup.Password).WithMessage("Password confirmation does not match the provided password.");

        RuleFor(x => x.Signup.Email)
            .NotNull().WithMessage("Email can not be empty!")
            .Matches(
                "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")
            .WithMessage("Not a valid email address.");

        RuleFor(x => x.Signup.NickName)
            .NotNull().WithMessage("Nickname can not be empty!");
    }
}

