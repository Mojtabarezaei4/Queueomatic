using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.Login;

public class LoginValidator : Validator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Login.Email)
            .NotNull().WithMessage("Email can not be empty!")
            .Matches(
                "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")
            .WithMessage("Not a valid email address.");

    }
}