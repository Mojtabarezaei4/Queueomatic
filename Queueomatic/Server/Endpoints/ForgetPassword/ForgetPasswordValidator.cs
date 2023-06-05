using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.ForgetPassword
{
    public class ForgetPasswordValidator : Validator<ForgetPasswordRequest>
    {
        public ForgetPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email can not be empty!")
                .Matches(
                    "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")
                .WithMessage("Not a valid email address!");
        }
    }
}
