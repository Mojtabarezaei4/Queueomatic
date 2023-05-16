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
        AllowAnonymous();       
    }

    public override async Task HandleAsync(GetUserByEmailRequest req, CancellationToken ct)
    {
        
            var response = new GetUserByEmailResponse(new UserDto());
            if (response is null) await SendAsync(new GetUserByEmailResponse(null), 404, cancellation: ct);
            else
            {
                await SendAsync(response, cancellation: ct);
            }
        
    }
}