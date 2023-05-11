using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Participant.GetAll;

public record GetAllParticipantResponse(IEnumerable<ParticipantDto> Participants);