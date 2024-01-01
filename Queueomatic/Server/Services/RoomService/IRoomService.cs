using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.RoomService;

public interface IRoomService
{
    public Task<Room?> CreateRoomAsync(string name, string userEmail);
    public RoomDto FromEntity(Room room);
    public IEnumerable<RoomDto> FromEntity(IEnumerable<Room> room);
    public Room ToEntity(RoomDto room);
    public IEnumerable<Room> ToEntity(IEnumerable<RoomDto> room); 
}