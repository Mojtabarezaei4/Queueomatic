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

    public IUnitOfWork UnitOfWork { get; set; }
    public IAuthenticationService AuthenticationService { get; set; }

    public override async Task HandleAsync(SignUpRequest req, CancellationToken ct)
    {
        req.Signup.NickName ??= string.Empty;

        if (await AuthenticationService.Register(req.Signup))
            await SendErrorsAsync(cancellation: ct);


        var response = new SignUpResponse();
        await SendCreatedAtAsync<SignUpEndpoint>("", response, cancellation: ct);

    }
}