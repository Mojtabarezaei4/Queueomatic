using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetAll;

public class GetAllRoomEndpoint : Endpoint<GetAllRoomRequest, GetAllRoomResponse>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/rooms");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllRoomRequest req, CancellationToken ct)
    {
        try
        {
            var response = new GetAllRoomResponse(new List<RoomDto>());
            await SendAsync(response, cancellation: ct);
        }
        catch (TaskCanceledException exception)
            when (exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(GetAllRoomEndpoint)} has cancelled");
        }
    }
}