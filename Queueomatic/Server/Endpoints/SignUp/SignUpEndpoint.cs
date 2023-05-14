using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.AuthenticationService;

namespace Queueomatic.Server.Endpoints.SignUp;

public class SignUpEndpoint: Endpoint<SignUpRequest, SignUpResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/signup");
        AllowAnonymous();
    }

    public IUnitOfWork UnitOfWork { get; set; }
    public IAuthenticationService AuthenticationService { get; set; }

    public override async Task HandleAsync(SignUpRequest req, CancellationToken ct)
    {
        try
        {
            
            var response = new SignUpResponse();
            await SendCreatedAtAsync<SignUpEndpoint>("", response, cancellation: ct);
        }
        catch (NullReferenceException nullException)
        {
            Logger.LogInformation($"The request can not be null.\nMessage: {nullException.Message}");
            await SendErrorsAsync(400, ct);
        }
        catch (TaskCanceledException exception)
            when(exception.CancellationToken == ct)
        {
            Logger.LogInformation($"Task {nameof(SignUpEndpoint)} was cancelled.");
            await SendErrorsAsync(400, ct);
        }
    }
}