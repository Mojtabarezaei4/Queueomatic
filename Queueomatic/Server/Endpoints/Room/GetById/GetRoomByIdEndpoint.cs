using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetById;

public class GetRoomByIdEndpoint : Endpoint<GetRoomByIdRequest, GetRoomByIdResponse>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/rooms/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRoomByIdRequest req, CancellationToken ct)
    {
        try
        {
            var response = new GetRoomByIdResponse(new RoomDto()
            {
                Name = "Test",
                Owner = null,
                Participators = new List<ParticipantDto>(),
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow,
                HashIds = req.Id
            });
            if (response is null) await SendAsync(new GetRoomByIdResponse(null), 404, cancellation: ct);
            else
            {
                await SendAsync(response, cancellation: ct);
            }
        }
        catch (TaskCanceledException exception)
            when (exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(GetRoomByIdEndpoint)} has cancelled.");
        }
    }
}