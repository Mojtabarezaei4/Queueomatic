using FastEndpoints;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.GetByEmail;

public class GetUserByEmailEndpoint: Endpoint<GetUserByEmailRequest, GetUserByEmailResponse>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/users/{email}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserByEmailRequest req, CancellationToken ct)
    {
        try
        {
            var response = new GetUserByEmailResponse(new UserDto());
            if (response is null) await SendAsync(new GetUserByEmailResponse(null), 404, cancellation: ct);
            else
            {
                await SendAsync(response, cancellation: ct);
            }
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(GetUserByEmailEndpoint)} was cancelled.");
        }
    }
}