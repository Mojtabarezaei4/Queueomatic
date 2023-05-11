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
            await SendAsync(response, 201, cancellation: ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(GetUserByEmailEndpoint)} was cancelled.");
        }
    }
}