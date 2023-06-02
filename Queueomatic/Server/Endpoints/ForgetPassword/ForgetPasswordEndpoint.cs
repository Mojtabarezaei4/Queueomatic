using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.AuthenticationService;
using Queueomatic.Server.Services.MailService;
using Queueomatic.Shared.DTOs;


namespace Queueomatic.Server.Endpoints.ForgetPassword;

public class ForgetPasswordEndpoint : Endpoint<ForgetPasswordRequest>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailService _mailService;
    public ForgetPasswordEndpoint(IAuthenticationService authenticationService, IConfiguration configuration, IUnitOfWork unitOfWork, IMailService mailService)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _mailService = mailService;
    }

    public override void Configure()
    {
        Post("/forgetPassword");
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
        user.ResetTokenExpires = DateTime.Now.AddDays(1);
        await _unitOfWork.SaveAsync();

        var url = $"{_configuration["AppUrl"]}/ResetPassword?email={req.Email}&token={user.PasswordResetToken}";

        var email = new EmailDto()
        {
            ToEmail = req.Email,
            Subject = "Reset Password",
            Body = "<h1>Follow the instructions to reset your password</h1>" +
                   $"<p>To reset your password <a href='{url}'>Click here</a></p>"
        };

        await _mailService.SendEmailAsync(email);

        await SendOkAsync("Link to reset password have sent to your email", ct);
    }
}