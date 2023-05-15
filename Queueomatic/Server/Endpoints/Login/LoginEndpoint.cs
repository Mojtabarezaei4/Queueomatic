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
        Post("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        if (!(await _authenticationService.CredentialsAreValid(req.Login.Email, req.Login.Password)))
            await SendAsync("The email or password were incorrect", 401);
    }
}