using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Participant.Add;

public record AddNewParticipantRequest(string RoomId, ParticipantDto Participant);