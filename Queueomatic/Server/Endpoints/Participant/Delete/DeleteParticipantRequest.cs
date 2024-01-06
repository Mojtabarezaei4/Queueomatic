using FastEndpoints;

namespace Queueomatic.Server.Endpoints.Participant.Delete;

public record DeleteParticipantRequest(Guid Id, 
    [property:FromClaim(IsRequired = false)] Guid? ParticipantId, 
    [property:FromClaim(IsRequired = false)] string? UserId);