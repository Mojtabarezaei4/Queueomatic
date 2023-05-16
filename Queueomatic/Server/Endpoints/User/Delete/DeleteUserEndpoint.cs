using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Server.Endpoints.User.Delete;

public class DeleteUserEndpoint: Endpoint<DeleteUserRequest, DeleteUserResponse>
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
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        await _unitOfWork.UserRepository.DeleteAsync(req.Email);
        await SendAsync(new DeleteUserResponse(), 200);
    }
}