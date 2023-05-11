using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Participant.GetAll;

public class GetAllParticipantEndpoint: Endpoint<GetAllParticipantRequest, GetAllParticipantResponse>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/rooms/{roomId}/participants");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllParticipantRequest req, CancellationToken ct)
    {
        try
        {
            var response = new GetAllParticipantResponse(new List<ParticipantDto>());
            await SendAsync(response, cancellation: ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(GetAllParticipantEndpoint)} was cancelled.");
        }
    }
}