using FastEndpoints;

namespace Queueomatic.Server.Endpoints.User.Delete;

public class DeleteUserEndpoint: Endpoint<DeleteUserRequest, DeleteUserResponse>
{
    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("/users/{email}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        try
        {
            var response = new DeleteUserResponse();
            await SendAsync(response, 204, cancellation: ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(DeleteUserEndpoint)} was cancelled.");
        }
    }
}