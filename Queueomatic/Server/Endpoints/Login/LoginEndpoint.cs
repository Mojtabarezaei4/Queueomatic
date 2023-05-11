using FastEndpoints;

namespace Queueomatic.Server.Endpoints.Login;

public class LoginEndpoint: Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        try
        {
            var response = new LoginResponse();
            await SendAsync(response, cancellation: ct);
        }
        catch (NullReferenceException nullException)
        {
            Logger.LogInformation($"The request can not be null.\nMessage: {nullException.Message}");
            await SendAsync(response:null, 400, ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(LoginEndpoint)} was cancelled.");
        }
    }
}