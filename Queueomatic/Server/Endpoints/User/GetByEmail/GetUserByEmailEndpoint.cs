using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.GetByEmail;

public class GetUserByEmailEndpoint: Endpoint<GetUserByEmailRequest, GetUserByEmailResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByEmailEndpoint(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("/users/{email}");
        Roles("Administrator", "User");
        Policies("SignedInUser");
    }

    public override async Task HandleAsync(GetUserByEmailRequest req, CancellationToken ct)
    {
        if (User.IsInRole("User") && !req.Email.Equals(req.UserId))
        {
            await SendUnauthorizedAsync();
            return;
        }

    }
    
}