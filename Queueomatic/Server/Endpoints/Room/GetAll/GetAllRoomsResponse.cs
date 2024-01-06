using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetAll;

public record GetAllRoomsResponse(IEnumerable<RoomDto> Rooms);