using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetAll;

public class GetAllRoomEndpoint : Endpoint<GetAllRoomRequest, GetAllRoomResponse>
{
    public override void Configure()
    {
        Get("/rooms");
        Roles("Administrator");
        
    }

    public override async Task HandleAsync(GetAllRoomRequest req, CancellationToken ct)
    {
       
    }
}