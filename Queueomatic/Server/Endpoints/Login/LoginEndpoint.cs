using FastEndpoints;

namespace Queueomatic.Server.Endpoints.Login;

public class LoginEndpoint: Endpoint<LoginRequest>
{
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