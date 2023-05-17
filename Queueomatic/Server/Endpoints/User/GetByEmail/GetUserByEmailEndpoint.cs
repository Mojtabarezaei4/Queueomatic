using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.RoomService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.User.GetByEmail;

public class GetUserByEmailEndpoint: Endpoint<GetUserByEmailRequest, GetUserByEmailResponse>
{
    private readonly IUnitOfWork _unitOfWork;
	private readonly IRoomService _roomService;

	public GetUserByEmailEndpoint(IUnitOfWork unitOfWork, IRoomService roomService)
	{
		_unitOfWork = unitOfWork;
		_roomService = roomService;
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
        var roomDtos = _roomService.ToEntity(user.Rooms);

        await SendAsync(new(new UserDto
        {
            Email = user.Email,
            NickName = user.NickName,
            Rooms = roomDtos
        }));
    }
}