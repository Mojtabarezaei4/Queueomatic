using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.AuthenticationService;

namespace Queueomatic.Server.Endpoints.ResetPassword;

public class ResetPasswordEndpoint : Endpoint<ResetPasswordRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthenticationService _authenticationService;

    public ResetPasswordEndpoint(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
    }

    public override void Configure()
    {
        Post("/reset-password");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        var user = await _unitOfWork.UserRepository.GetUserByToken(req.Request.Token);
        if (user is null || user.ResetTokenExpires < DateTime.Now)
        {
            await SendErrorsAsync();
        }

        _authenticationService.CreatePasswordHash(req.Request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.PasswordResetToken = null;
        user.ResetTokenExpires = null;

        await _unitOfWork.SaveAsync();

        await SendOkAsync("Password reset successfully!");
    }
}