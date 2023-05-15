using FastEndpoints;
using Queueomatic.Server.Services.AuthenticationService;

namespace Queueomatic.Server.Endpoints.SignUp;

public class SignUpEndpoint : Endpoint<SignUpRequest>
{
    private readonly IAuthenticationService _authenticationService;

    public SignUpEndpoint(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/signup");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(SignUpRequest req, CancellationToken ct)
    {
        req.Signup.NickName ??= string.Empty;

        if (!await _authenticationService.Register(req.Signup))
        {
            await SendAsync("User already exists.", 400);
            return; 
        }

        await SendCreatedAtAsync<SignUpEndpoint>("api/login", "Successfully registered!");
    }
}