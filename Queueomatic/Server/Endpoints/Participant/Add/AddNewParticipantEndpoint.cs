using FastEndpoints;

namespace Queueomatic.Server.Endpoints.Participant.Add;

public class AddNewParticipantEndpoint: Endpoint<AddNewParticipantRequest, AddNewParticipantResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/rooms/{roomId}/newParticipant");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddNewParticipantRequest req, CancellationToken ct)
    {
        try
        {
            var response = new AddNewParticipantResponse();
            await SendAsync(response, 201, cancellation: ct);
        }
        catch (NullReferenceException nullException)
        {
            Logger.LogInformation($"The request can not be null.\nMessage: {nullException.Message}");
            await SendAsync(response:null, 400, ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(AddNewParticipantEndpoint)} was cancelled.");
        }
    }
}