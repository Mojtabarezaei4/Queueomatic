using FastEndpoints;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Queueomatic.Server.Endpoints.Room.Add;

public class AddNewRoomValidator : Validator<AddNewRoomRequest>
{
    public AddNewRoomValidator()
    {
        When(x => x.UserId.IsNullOrEmpty(), () =>
        {
            RuleFor(x => x.UserId)
                .Null()
                .WithMessage("Email address can not be empty!");
        }).Otherwise(() =>
        {
            RuleFor(x => x.UserId)
                .Matches(
                    "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")
                .WithMessage("Not a valid email address!");
        });

        When(x => !x.Name.IsNullOrEmpty(), () =>
        {
            RuleFor(x => x.Name)
                .Length(1, 20)
                .WithMessage("The room name can be 20 characters at max.");
        });
    }
}
