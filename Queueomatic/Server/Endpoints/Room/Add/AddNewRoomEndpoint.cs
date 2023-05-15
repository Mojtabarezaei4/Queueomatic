using FastEndpoints;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;
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
        Verbs(Http.POST);
        Routes("/addNewRoom");
        AllowAnonymous();
        // Policies("SignedInUser");
    }

    public override async Task HandleAsync(AddNewRoomRequest req, CancellationToken ct)
    {
        var roomCreated = await _createRoomService.CreateRoomAsync(req.Room, req.UserEmail);

        if (roomCreated is true)
        {
            await SendAsync(new AddNewRoomResponse(), 201, cancellation: ct);
            return;
        }
        
        await SendAsync("Something went wrong.",400, cancellation: ct);
    }
}