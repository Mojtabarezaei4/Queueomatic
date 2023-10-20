using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Queueomatic.DataAccess.Models;
using Queueomatic.Server.Services.CacheRoomService;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Models;

namespace Queueomatic.Server.Endpoints.Hubs.Room;

public class RoomHub : Hub
{
    private readonly ICacheRoomService _cacheService;
    public RoomHub(ICacheRoomService cacheService)
    {
        _cacheService = cacheService ?? throw new ArgumentException($"The value of cache cannot be null");
    }

    public async Task UpdateParticipant(ParticipantRoomDto participant, StatusDto status, string roomId)
    {
        var participantToBeUpdated = _cacheService.UpdateRoom(participant, status, roomId);
        await Clients.Groups(roomId).SendAsync("MoveParticipant", participantToBeUpdated, status);
    }

    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public async Task<RoomModel> InitializeParticipant(ParticipantRoomDto participant, string roomId, string roomName)
    {
        if (_cacheService.InitRoom(roomId, roomName, participant))
            await Clients.Groups(roomId).SendAsync("MoveParticipant", participant, Status.Idling);
        return _cacheService.GetRoom(roomId);
    }

    public async Task LeaveRoom(ParticipantRoomDto participant, string roomId)
    {
        await Clients.Groups(roomId).SendAsync("ClearTheRoom", participant);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        _cacheService.CleanRoom(participant, roomId);
    }
}