using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.Add;

public record AddNewRoomRequest(string Name, [property: FromClaim]string UserId);