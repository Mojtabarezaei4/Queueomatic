using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetAll;

public record GetAllRoomResponse(IEnumerable<RoomDto> Rooms);