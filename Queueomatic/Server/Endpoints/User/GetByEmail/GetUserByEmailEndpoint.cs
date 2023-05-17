using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.GetByEmail;

public class GetUserByEmailEndpoint: Endpoint<GetUserByEmailRequest, GetUserByEmailResponse>
{
    private readonly IUnitOfWork _unitOfWork;
	private readonly IHashIdService _hashIdService;

	public GetUserByEmailEndpoint(IUnitOfWork unitOfWork, IHashIdService hashIdService)
	{
		_unitOfWork = unitOfWork;
		_hashIdService = hashIdService;
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
        
        var user = await _unitOfWork.UserRepository.GetAsync(req.Email);
        var roomDto = new RoomDto
        {
            Name = user.Rooms.
        }

        await SendAsync(new(new UserDto
        {
            Email = user.Email,
            NickName = user.NickName,
            Rooms = user.Rooms
        }));
    }
    
}