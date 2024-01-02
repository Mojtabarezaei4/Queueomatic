using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Server.Endpoints.User.ChangeUsername;

public class ChangeUsernameEndpoint : Endpoint<ChangeUsernameRequest, ChangeUsernameResponse>
{

    private readonly IUnitOfWork _unitOfWork;

    public ChangeUsernameEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Post("/user/username");
        Policies("SignedInUser");
    }

    public override async Task HandleAsync(ChangeUsernameRequest req, CancellationToken ct)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(req.UserId);
        if (user == null)
        {
            await SendNotFoundAsync();
            return;
        }

        user.NickName = req.Username;
        await _unitOfWork.UserRepository.UpdateAsync(user);
        var result = await _unitOfWork.SaveAsync();

        if (result <= 0)
        {
            await SendErrorsAsync();
            return;
        }

        await SendOkAsync(new ChangeUsernameResponse { UpdatedUsername = user.NickName });
    }
}