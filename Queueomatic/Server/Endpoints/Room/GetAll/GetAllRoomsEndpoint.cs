using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.RoomService;

namespace Queueomatic.Server.Endpoints.Room.GetAll;

public class GetAllRoomsEndpoint : Endpoint<GetAllRoomRequest, GetAllRoomsResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomService _roomService;
    private readonly IHashIdService _hashIdService;

    public GetAllRoomsEndpoint(IUnitOfWork unitOfWork, IRoomService roomService, IHashIdService hashIdService)
    {
        _unitOfWork = unitOfWork;
        _roomService = roomService;
        _hashIdService = hashIdService;
    }

    public override void Configure()
    {
        Get("/rooms");
        Roles("Administrator");
    }

    public override async Task HandleAsync(GetAllRoomRequest req, CancellationToken ct)
    {
        var roomId = _hashIdService.Decode(req.RoomId);
        var roomName = req.RoomName == null ? "" : req.RoomName;
        var rooms = await _unitOfWork.RoomRepository.GetAllAsync(roomId, roomName);

        await SendAsync(new GetAllRoomsResponse(_roomService.FromEntity(rooms)));
    }
}