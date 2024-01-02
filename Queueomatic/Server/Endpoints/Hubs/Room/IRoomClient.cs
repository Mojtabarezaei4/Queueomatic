using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Hubs.Room;

public interface IRoomClient
{
    public Task MoveParticipant(ParticipantRoomDto participantToBeUpdated, Status status);
    public Task ClearRoom(ParticipantRoomDto participant);
    public Task KickParticipant();
}