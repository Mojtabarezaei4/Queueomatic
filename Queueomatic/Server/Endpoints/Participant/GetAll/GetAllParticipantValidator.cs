using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.Participant.GetAll;

public class GetAllParticipantValidator : Validator<GetAllParticipantRequest>
{
    public GetAllParticipantValidator()
    {
        RuleFor(x => x.RoomId)
            .MinimumLength(6)
            .NotNull()
            .NotEmpty();
    }
}