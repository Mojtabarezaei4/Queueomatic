using FastEndpoints;
using Queueomatic.Server.Services.CreateRoomService;

namespace Queueomatic.Server.Endpoints.Room.Add;

public class AddNewRoomEndpoint : Endpoint<AddNewRoomRequest>
{
    private readonly ICreateRoomService _createRoomService;

    public AddNewRoomEndpoint(ICreateRoomService createRoomService)
    {
        _createRoomService = createRoomService;
    }
    
    public override void Configure()
    {
        Post("/addNewRoom");
        Description(x => x.WithName("AddNewRoom"));
        Policies("SignedInUser");
    }

    public override async Task HandleAsync(AddNewRoomRequest req, CancellationToken ct)
    {
            var roomCreated = await _createRoomService.CreateRoomAsync(req.Room, req.UserId);

        if (roomCreated == false)
        {
            ThrowError("Something went wrong.");
            return;
        }
        await SendCreatedAtAsync<AddNewRoomEndpoint>("AddNewRoom", "Room created");
    }
}