using FastEndpoints;

namespace Queueomatic.Server.Endpoints.Participant.Delete;

public class DeleteParticipantEndpoint: Endpoint<DeleteParticipantRequest, DeleteParticipantResponse>
{
    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("/participants/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteParticipantRequest req, CancellationToken ct)
    {
        try
        {
            var response = new DeleteParticipantResponse();
            await SendAsync(response, 204, cancellation: ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(DeleteParticipantEndpoint)} was cancelled.");
        }   
    }
}