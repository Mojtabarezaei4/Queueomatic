using FastEndpoints;
using FluentValidation;

namespace Queueomatic.Server.Endpoints.Participant.Delete;

public class DeleteParticipantValidator : Validator<DeleteParticipantRequest>
{
    public DeleteParticipantValidator()
    {
        RuleFor(expression => expression.Id)
        .NotNull()
        .NotEmpty();
    }
}