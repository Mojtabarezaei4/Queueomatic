using FastEndpoints;

namespace Queueomatic.Server.Endpoints.User.Edit;

public class EditUserEndpoint: Endpoint<EditUserRequest, EditUserResponse>
{
    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/users/{email}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EditUserRequest req, CancellationToken ct)
    {
        try
        {
            var response = new EditUserResponse();
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
            Logger.LogInformation($"Task {nameof(EditUserEndpoint)} was cancelled.");
        }
    }
}