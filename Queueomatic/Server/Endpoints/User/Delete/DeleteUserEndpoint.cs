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
        Verbs(Http.DELETE);
        Routes("/users/{email}");
        Policies("SignedInUser");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        var user =  _unitOfWork.UserRepository.DeleteAsync(req.Email);
        if (user is null)
        {
            await SendNotFoundAsync();
        }
        else
        {
            await SendOkAsync("Account deleted");
        }
    }
}