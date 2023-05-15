using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.CreateRoomService;

public class CreateRoomService : ICreateRoomService
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreateRoomAsync(RoomDto room, string userEmail)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(userEmail);

        if (user is null) return false;

        var roomModel = new Room()
        {
            Name = string.IsNullOrEmpty(room.Name) ? "Random Funny Name" : room.Name,
            Participators = new List<Participant>(),
            Owner = user
        };

        await _unitOfWork.RoomRepository.AddAsync(roomModel);
        await _unitOfWork.SaveAsync();
        return true;
    }
}