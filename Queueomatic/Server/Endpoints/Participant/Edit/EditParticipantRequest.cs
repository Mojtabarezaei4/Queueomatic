using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Participant.Edit;

public record EditParticipantRequest(Guid Id, ParticipantDto Participant);