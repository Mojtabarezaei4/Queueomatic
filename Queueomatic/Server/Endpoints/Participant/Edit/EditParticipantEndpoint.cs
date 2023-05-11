using FastEndpoints;

namespace Queueomatic.Server.Endpoints.Participant.Edit;

public class EditParticipantEndpoint: Endpoint<EditParticipantRequest, EditParticipantResponse>
{
    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/participants/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EditParticipantRequest req, CancellationToken ct)
    {
        
        try
        {
            var response = new EditParticipantResponse();
            await SendAsync(response, 204, cancellation: ct);
        }
        catch (NullReferenceException nullException)
        {
            Logger.LogInformation($"The request can not be null.\nMessage: {nullException.Message}");
            await SendAsync(response:null, 400, ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(EditParticipantEndpoint)} was cancelled.");
        }
    }
}