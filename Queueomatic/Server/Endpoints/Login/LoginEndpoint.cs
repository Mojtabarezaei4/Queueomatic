using FastEndpoints;
using Queueomatic.Server.Services.AuthenticationService;

namespace Queueomatic.Server.Endpoints.Login;

public class LoginEndpoint: Endpoint<LoginRequest>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginEndpoint(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
       
    }
}