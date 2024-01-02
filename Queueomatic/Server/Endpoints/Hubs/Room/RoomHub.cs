using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Queueomatic.Server.Services.CacheRoomService;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Models;

namespace Queueomatic.Server.Endpoints.Hubs.Room;

public class RoomHub : Hub<IRoomClient>
{
    private readonly ICacheRoomService _cacheService;
    public RoomHub(ICacheRoomService cacheService)
    {
        _cacheService = cacheService ?? throw new ArgumentException($"The value of cache cannot be null");
    }

    public async Task UpdateParticipant(ParticipantRoomDto participant, StatusDto status, string roomId)
    {
        var participantToBeUpdated = _cacheService.UpdateRoom(participant, status, roomId);
        await Clients.Groups(roomId).MoveParticipant(participantToBeUpdated, status);
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task<RoomModel> InitializeParticipant(ParticipantRoomDto participant, string roomId, string roomName)
    {
        _cacheService.InitRoom(roomId, roomName, participant);
 
        if(_cacheService.GetParticipant(participant.Id, roomId) is ParticipantRoomDto cachedParticipant)
            participant.Status = cachedParticipant.Status;

        _cacheService.UpdateRoom(participant, participant.Status, roomId);

        await Clients.Groups(roomId).MoveParticipant(participant, participant.Status);
        return _cacheService.GetRoom(roomId);
    }

    public async Task LeaveRoom(ParticipantRoomDto? participant, string roomId, string? connectionId = null)
    {
        if (string.IsNullOrWhiteSpace(connectionId))
            connectionId = null;

        await Clients.Groups(roomId).ClearRoom(participant);
        await Groups.RemoveFromGroupAsync(connectionId ?? Context.ConnectionId, roomId);

        if(participant != null)
            _cacheService.CleanRoom(participant, roomId);
    }

    public async Task<RoomModel> GetState(string roomId)
    {
        return await Task.Run(() => _cacheService.GetRoom(roomId));
    }

    [Authorize("VerifyOwnership")]
    public async Task KickParticipant(string roomId, string connectionIdOfParticipant, Guid participantId)
    {
        var participant = _cacheService.GetParticipant(participantId, roomId);
        await LeaveRoom(participant, roomId, connectionIdOfParticipant);
        await Clients.Client(connectionIdOfParticipant).KickParticipant();
    }
}