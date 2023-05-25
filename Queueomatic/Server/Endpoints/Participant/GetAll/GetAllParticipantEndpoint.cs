using FastEndpoints;
using Queueomatic.Server.Services.ParticipantService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Participant.GetAll;

public class GetAllParticipantEndpoint: Endpoint<GetAllParticipantRequest, GetAllParticipantResponse>
{
    private readonly IParticipantService _participantService;

    public GetAllParticipantEndpoint(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    public override void Configure()
    {
        Get("/rooms/{roomId}/participants");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllParticipantRequest req, CancellationToken ct)
    {
        var result = await _participantService.GetAllAsync(req.RoomId);
        if (result == null)
        {
            await SendErrorsAsync();
            return;
        }
        await SendOkAsync(new GetAllParticipantResponse(result));
    }
}