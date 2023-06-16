using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.AuthenticationService;
using Queueomatic.Server.Services.MailService;


namespace Queueomatic.Server.Endpoints.ForgetPassword;

public class ForgetPasswordEndpoint : Endpoint<ForgetPasswordRequest>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailService _mailService;
    public ForgetPasswordEndpoint(IAuthenticationService authenticationService, IUnitOfWork unitOfWork, IMailService mailService)
    {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
        _mailService = mailService;
    }

    public override void Configure()
    {
        Post("/forgotPassword");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ForgetPasswordRequest req, CancellationToken ct)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(req.Email);
        if (user is null)
        {
            await SendNotFoundAsync();
            return;
        }

        var token = _authenticationService.CreateRandomToken();
        user.PasswordResetToken = token;
        user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);
        await _unitOfWork.SaveAsync();


        await _mailService.SendEmailAsync(_mailService.CreateEmail(req.Email,user), req.Email);

        await SendOkAsync("Link to reset password have sent to your email", ct);
    }
}