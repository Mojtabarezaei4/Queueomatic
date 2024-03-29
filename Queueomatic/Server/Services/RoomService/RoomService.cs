﻿using Queueomatic.DataAccess.Models;
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

    public async Task<Room?> CreateRoomAsync(string name, string userEmail)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(userEmail);

        if (user is null) return null;

        var roomModel = new Room
        {
            Name = string.IsNullOrEmpty(name) ? GetName() : name,
            Participants = new List<Participant>(),
            Owner = user,
            CreatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddDays(1)
        };

        await _unitOfWork.RoomRepository.AddAsync(roomModel);
        await _unitOfWork.SaveAsync();
        return roomModel;
    }

    public RoomDto FromEntity(Room room)
    {
		return new RoomDto
		{
			HashId = _hashIdService.Encode(room.Id),
			Name = room.Name,
			Owner = new UserDto
			{
				Email = room.Owner.Email,
				NickName = room.Owner.NickName
			},
			Participators = room.Participants.Select(p => new ParticipantDto
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

    public IEnumerable<RoomDto> FromEntity(IEnumerable<Room> room)
    {
	    return room.Select(FromEntity);
    }

    public Room ToEntity(RoomDto room)
    {
		return new Room
		{
			Id = _hashIdService.Decode(room.HashId),
			Name = room.Name,
			Owner = new User
			{
				Email = room.Owner.Email,
				NickName = room.Owner.NickName
			},
			Participants = (ICollection<Participant>)room.Participators.Select(p => new Participant
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

    public IEnumerable<Room> ToEntity(IEnumerable<RoomDto> room)
    {
	    return room.Select(ToEntity);
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