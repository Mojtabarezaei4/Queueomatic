using FastEndpoints;
using FastEndpoints.Security;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.AuthenticationService;

namespace Queueomatic.Server.Endpoints.Login;

public class LoginEndpoint : Endpoint<LoginRequest>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public LoginEndpoint(IAuthenticationService authenticationService, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
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



        var jwtToken = JWTBearer.CreateToken(
            signingKey: _configuration.GetSection("JWTSigningKeys").GetSection("DefaultKey").Value!,
            expireAt: DateTime.UtcNow.AddDays(1),
            priviledges: u =>
            {
                u.Roles
            });
    }
}