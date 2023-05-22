using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.AuthenticationService;

namespace Queueomatic.Server.Endpoints.User.Delete;

public class DeleteUserEndpoint: Endpoint<DeleteUserRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override void Configure()
    {
        Delete("/users/{email}");
        Roles("User");
        Policies("SignedInUser");
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {

        if (!User.IsInRole("User") && !req.Email.Equals(req.UserId))
        {
            await SendUnauthorizedAsync();
            return;
        }

        await  _unitOfWork.UserRepository.DeleteAsync(req.Email);
        var result = await _unitOfWork.SaveAsync();
        if (result == 0)
        {
            await SendNotFoundAsync();
            return;
        }
        await SendNoContentAsync();
        
    }
}