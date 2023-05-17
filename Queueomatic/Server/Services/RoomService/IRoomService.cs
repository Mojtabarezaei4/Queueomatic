using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.RoomService;

public interface IRoomService
{
    public Task<bool> CreateRoomAsync(RoomDto room, string userEmail);
}