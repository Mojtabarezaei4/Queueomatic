using FastEndpoints;
using HashidsNet;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.RoomService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Endpoints.Room.GetById;

public class GetRoomByIdEndpoint : Endpoint<GetRoomByIdRequest, GetRoomByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomService _roomService;
    private readonly IHashIdService _hashIdService;

    public GetRoomByIdEndpoint(IUnitOfWork unitOfWork, IRoomService roomService, IHashIdService hashIdService)
    {
        _unitOfWork = unitOfWork;
        _roomService = roomService;
        _hashIdService = hashIdService;
    }

    public override void Configure()
    {
        Get("/rooms/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRoomByIdRequest req, CancellationToken ct)
    {
        try
        {
            var decodedRoomId = _hashIdService.Decode(req.Id);
            var room = await _unitOfWork.RoomRepository.GetAsync(decodedRoomId);
            if (room is null)
            {
                await SendNotFoundAsync();
                return;
            }

            var response = _roomService.FromEntity(room);
            await SendAsync(new(response), cancellation: ct);
        }
        catch (NoResultException exception)
        {
            await SendNotFoundAsync();
        }
    }
}