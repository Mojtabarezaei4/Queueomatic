using Microsoft.AspNetCore.SignalR;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Hubs.Room;

public class RoomHub : Hub
{
    public async Task UpdateParticipant(ParticipantRoomDto participant, StatusDto status, string roomId)
    {
        participant.StatusDate = DateTime.UtcNow;
        await Clients.Groups(roomId).SendAsync("MoveParticipant", participant, status);
    }

    public Task JoinRoom(string roomName)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public Task LeaveRoom(string roomName)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }
}