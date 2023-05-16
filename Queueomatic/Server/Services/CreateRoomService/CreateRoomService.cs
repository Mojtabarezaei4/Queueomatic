using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Resources;

namespace Queueomatic.Server.Services.CreateRoomService;

public class CreateRoomService : ICreateRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Random _random;

    public CreateRoomService(IUnitOfWork unitOfWork, Random random)
    {
        _unitOfWork = unitOfWork;
        _random = random;
    }

    public async Task<bool> CreateRoomAsync(RoomDto room, string userEmail)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(userEmail);

        if (user is null) return false;

        var roomModel = new Room()
        {
            Name = string.IsNullOrEmpty(room.Name) ? GetName() : room.Name,
            Participators = new List<Participant>(),
            Owner = user
        };

        await _unitOfWork.RoomRepository.AddAsync(roomModel);
        await _unitOfWork.SaveAsync();
        return true;
    }

    private string GetName()
    {
        string name = String.Empty;

        var randomIndex = _random.Next(0, Enum.GetValues<Adjectives>().Length);
        
        name = Enum.GetValues<Adjectives>().GetValue(randomIndex) + " ";

        randomIndex = _random.Next(0, Enum.GetValues<Nouns>().Length);

        name += Enum.GetValues<Nouns>().GetValue(randomIndex)!.ToString();

        if (name.Length > 20) GetName();

        return name;
    }
}