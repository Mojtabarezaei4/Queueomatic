using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.Room.GetById;

public class GetRoomByIdValidator : Validator<GetRoomByIdRequest>
{
    public GetRoomByIdValidator()
    {
        RuleFor(x => x.Id)
            .Length(6)
            .NotNull()
            .NotEmpty();
    }
}