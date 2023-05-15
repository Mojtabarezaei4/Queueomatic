using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.Add;

public record AddNewRoomRequest(RoomDto Room, string UserEmail);