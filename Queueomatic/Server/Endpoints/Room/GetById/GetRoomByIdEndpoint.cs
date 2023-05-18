using FastEndpoints;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.RoomService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetById;

public class GetRoomByIdEndpoint : Endpoint<GetRoomByIdRequest, GetRoomByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomService _roomService;

    public GetRoomByIdEndpoint(IUnitOfWork unitOfWork, IRoomService roomService)
    {
        _unitOfWork = unitOfWork;
        _roomService = roomService;
    }

    public override void Configure()
    {
        Get("/rooms/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRoomByIdRequest req, CancellationToken ct)
    {
        // var response = _unitOfWork.RoomRepository.GetAsync(_roomService.ToEntity(req.Id)));
        
        await SendAsync(null, cancellation: ct);
    }
}