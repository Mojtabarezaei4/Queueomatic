using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.RoomService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetAll;

public class GetAllRoomsEndpoint : Endpoint<GetAllRoomRequest, GetAllRoomsResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomService _roomService;

    public GetAllRoomsEndpoint(IUnitOfWork unitOfWork, IRoomService roomService)
    {
        _unitOfWork = unitOfWork;
        _roomService = roomService;
    }

    public override void Configure()
    {
        Get("/rooms");
        Roles("Administrator");
    }

    public override async Task HandleAsync(GetAllRoomRequest req, CancellationToken ct)
    {
        var rooms = await _unitOfWork.RoomRepository.GetAllAsync();
    }
}