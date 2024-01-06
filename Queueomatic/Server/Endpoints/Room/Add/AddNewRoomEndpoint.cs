using FastEndpoints;
using Queueomatic.Server.Services.RoomService;

namespace Queueomatic.Server.Endpoints.Room.Add;

public class AddNewRoomEndpoint : Endpoint<AddNewRoomRequest, AddNewRoomResponse>
{
	private readonly IRoomService _roomService;

	public AddNewRoomEndpoint(IRoomService roomService)
	{
		_roomService = roomService;
	}

	public override void Configure()
	{
		Post("/room/create");
		Description(x => x.WithName("AddNewRoom"));
		Policies("SignedInUser");
	}

	public override async Task HandleAsync(AddNewRoomRequest req, CancellationToken ct)
	{
		var createdRoom = await _roomService.CreateRoomAsync(req.Name, req.UserId);

		if (createdRoom == null)
		{
			ThrowError("Something went wrong.");
			return;
		}

        var roomHashIds = _roomService.FromEntity(createdRoom).HashId;

        await SendCreatedAtAsync<AddNewRoomEndpoint>("AddNewRoom", new AddNewRoomResponse("Room created", roomHashIds));
	}
}