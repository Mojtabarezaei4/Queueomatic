using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Hubs.Room;

public interface IRoomClient
{
    public Task MoveParticipant(ParticipantRoomDto participantToBeUpdated, StatusDto status);
    public Task ClearRoom(ParticipantRoomDto participant);
    public Task KickParticipant();
}