using FastEndpoints;
using Queueomatic.Server.Services.RoomService;

namespace Queueomatic.Server.Endpoints.Room.Add;

public class AddNewRoomEndpoint : Endpoint<AddNewRoomRequest>
{
    private readonly IRoomService _roomService;

    public AddNewRoomEndpoint(IRoomService roomService)
    {
        _roomService = roomService;
    }
    
    public override void Configure()
    {
        Post("/addNewRoom");
        Description(x => x.WithName("AddNewRoom"));
        Policies("SignedInUser");
    }

    public override async Task HandleAsync(AddNewRoomRequest req, CancellationToken ct)
    {
            var roomCreated = await _roomService.CreateRoomAsync(req.Room, req.UserId);

        if (roomCreated == false)
        {
            ThrowError("Something went wrong.");
            return;
        }
        await SendCreatedAtAsync<AddNewRoomEndpoint>("AddNewRoom", "Room created");
    }
}