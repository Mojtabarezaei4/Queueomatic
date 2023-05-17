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
        Policies("SignedInUser");
    }

    public override async Task HandleAsync(AddNewRoomRequest req, CancellationToken ct)
    {
            var roomCreated = await _createRoomService.CreateRoomAsync(req.Room, req.UserEmail);

        if (roomCreated == false)
        {
            await SendAsync("Something went wrong.",400);
            return;
        }
        await SendAsync(new AddNewRoomResponse(), 201);
    }
}