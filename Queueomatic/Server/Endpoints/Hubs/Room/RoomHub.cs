using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Models;

namespace Queueomatic.Server.Endpoints.Hubs.Room;

public class RoomHub : Hub
{
    private readonly IMemoryCache _cache;
    public RoomHub(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentException($"The value of cache cannot be null");
    }

    public async Task UpdateParticipant(ParticipantRoomDto participant, StatusDto status, string roomId)
    {
        participant.StatusDate = DateTime.UtcNow;
        var room = _cache.Get<RoomModel>(roomId);

        var activeList = GetList(status, room);
        activeList.Add(participant);

        var oldList = GetList(participant.Status, room);
        oldList.RemoveAll(p => p.Id == participant.Id);

        _cache.Set(roomId, room);
        
       // await Clients.Groups(roomId).SendAsync("ClearTheRoom", participant);
        await Clients.Groups(roomId).SendAsync("MoveParticipant", participant, status);
    }

    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public async Task<RoomModel> InitializeParticipant(ParticipantRoomDto participant, string roomId, string roomName)
    {
        RoomModel room;
        if (!_cache.TryGetValue(roomId, out _))
        {
            room = new RoomModel
            {
                RoomId = roomId,
                RoomName = roomName,
                IdlingParticipants = new(),
                WaitingParticipants = new(),
                ActiveParticipants = new()
            };
        }
        else
            room = _cache.Get<RoomModel>(roomId)!;

        if (!VerifyParticipant(participant, room))
        {
            participant.StatusDate = DateTime.UtcNow;
            room!.IdlingParticipants.Add(participant);
            _cache.Set(roomId, room);
            await Clients.Groups(roomId).SendAsync("MoveParticipant", participant, Status.Idling);
        }
        else
        {
            await Clients.Groups(roomId).SendAsync("UpdateRoom", room);
        }

        return room;
    }

    private bool VerifyParticipant(ParticipantRoomDto participant, RoomModel room)
    {
        return room.IdlingParticipants.Any(p => p.Id == participant.Id) ||
               room.WaitingParticipants.Any(p => p.Id == participant.Id) ||
               room.ActiveParticipants.Any(p => p.Id == participant.Id);
    }

    public async Task LeaveRoom(ParticipantRoomDto participant, string roomId)
    {
        await Clients.Groups(roomId).SendAsync("ClearTheRoom", participant);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        RemoveParticipantFromRoom(participant, roomId);
    }

    private void RemoveParticipantFromRoom(ParticipantRoomDto participantRoomDto, string roomId)
    {
        var room = _cache.Get<RoomModel>(roomId);
        room.IdlingParticipants.RemoveAll(x => x.Id == participantRoomDto.Id);
        room.WaitingParticipants.RemoveAll(x => x.Id == participantRoomDto.Id);
        room.ActiveParticipants.RemoveAll(x => x.Id == participantRoomDto.Id);

        _cache.Set(roomId, room);
    }

    private List<ParticipantRoomDto> GetList(StatusDto participantStatus, RoomModel room)
    {
        return participantStatus switch
        {
            StatusDto.Idling => room.IdlingParticipants,
            StatusDto.Waiting => room.WaitingParticipants,
            StatusDto.Ongoing => room.ActiveParticipants,
            _ => throw new ArgumentOutOfRangeException(nameof(participantStatus), participantStatus, null)
        };
    }

}