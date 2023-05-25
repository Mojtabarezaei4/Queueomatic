using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.Participant.Add;

public class AddNewParticipantValidator : Validator<AddNewParticipantRequest>
{
    public AddNewParticipantValidator()
    {
        RuleFor(expression => expression.Participant.NickName)
            .NotNull()
            .NotEmpty();
    }
}