using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.Room.Add;

public class AddNewRoomValidator : Validator<AddNewRoomRequest>
{
    public AddNewRoomValidator()
    {
        RuleFor(x => x.UserEmail)
            .Matches(
                "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")
            .WithMessage("Not a valid email address!");
    }
}