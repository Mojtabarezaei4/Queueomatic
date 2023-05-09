using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetAll;

public class GetAllRoomEndpoint : Endpoint<GetAllRoomRequest, GetAllRoomResponse>
{
    public ILogger<GetAllRoomEndpoint> Logger { get; init; }
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
            await SendAsync(new GetAllRoomResponse(new List<RoomDto>()), cancellation: ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(GetAllRoomEndpoint)} has cancelled");
        }
    }
}