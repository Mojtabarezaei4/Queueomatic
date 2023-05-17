using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.Add;

public record AddNewRoomRequest(RoomDto Room, [property: FromClaim]string UserId);