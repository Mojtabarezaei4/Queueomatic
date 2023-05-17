using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Resources;

namespace Queueomatic.Server.Services.RoomService;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly Random _random;
	private readonly IHashIdService _hashIdService;

	public RoomService(IUnitOfWork unitOfWork, Random random, IHashIdService hashIdService)
    {
        _unitOfWork = unitOfWork;
        _random = random;
        _hashIdService = hashIdService;
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

    public RoomDto ToEntity(Room room)
    {
		return new RoomDto
		{
			HashIds = _hashIdService.Encode(room.Id),
			Name = room.Name,
			Owner = new UserDto
			{
				Email = room.Owner.Email,
				NickName = room.Owner.NickName
			},
			Participators = room.Participators.Select(p => new ParticipantDto
			{
				Id = p.Id,
				NickName = p.NickName,
				StatusDate = p.StatusDate, 
				Status = (StatusDto)p.Status
			}),
            CreatedAt = room.CreatedAt,
            ExpireAt = room.ExpireAt
		};
	}

    public IEnumerable<RoomDto> ToEntity(IEnumerable<Room> room)
    {
	    return room.Select(ToEntity);
    }

    public Room FromEntity(RoomDto room)
    {
		return new Room
		{
			Id = _hashIdService.Decode(room.HashIds),
			Name = room.Name,
			Owner = new User
			{
				Email = room.Owner.Email,
				NickName = room.Owner.NickName
			},
			Participators = (ICollection<Participant>)room.Participators.Select(p => new Participant
			{
				Id = p.Id,
				NickName = p.NickName,
				StatusDate = p.StatusDate,
				Status = (Status)p.Status
			}),
			CreatedAt = room.CreatedAt,
			ExpireAt = room.ExpireAt
		};
	}

    public IEnumerable<Room> FromEntity(IEnumerable<RoomDto> room)
    {
	    return room.Select(FromEntity);
    }

    private string GetName()
    {
        string name;

        var randomIndex = _random.Next(0, Enum.GetValues<Adjectives>().Length);
        
        name = Enum.GetValues<Adjectives>().GetValue(randomIndex) + " ";

        randomIndex = _random.Next(0, Enum.GetValues<Nouns>().Length);

        name += Enum.GetValues<Nouns>().GetValue(randomIndex)!.ToString();

        if (name.Length > 20) GetName();

        return name;
    }
}