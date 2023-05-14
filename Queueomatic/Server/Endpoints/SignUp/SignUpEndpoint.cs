using System.Text;
using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.AuthenticationService;

namespace Queueomatic.Server.Endpoints.SignUp;

public class SignUpEndpoint : Endpoint<SignUpRequest, SignUpResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/signup");
        AllowAnonymous();
    }
    public IAuthenticationService AuthenticationService { get; set; }

    public override async Task HandleAsync(SignUpRequest req, CancellationToken ct)
    {
        req.Signup.NickName ??= string.Empty;

        var response = new SignUpResponse();

        if (!await AuthenticationService.Register(req.Signup))
        {
            await SendAsync(response, 400, cancellation: ct);
            return; 
        }

        await SendCreatedAtAsync<SignUpEndpoint>("", response, cancellation: ct);
    }
}