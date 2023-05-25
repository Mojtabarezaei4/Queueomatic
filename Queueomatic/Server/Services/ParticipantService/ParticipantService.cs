using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.ParticipantService;

public class ParticipantService : IParticipantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashIdService _hashIdService;

    public ParticipantService(IUnitOfWork unitOfWork, IHashIdService hashIdService)
    {
        _unitOfWork = unitOfWork;
        _hashIdService = hashIdService;
    }


    public async Task<ParticipantDto?> CreateOneAsync(ParticipantDto participantDto, string roomId)
    {
        var room = await _unitOfWork.RoomRepository.GetAsync(_hashIdService.Decode(roomId));
        if (room is null) return null;
        
        var participant = new Participant
        {
            NickName = participantDto.NickName,
            Room = room,
            StatusDate = DateTime.UtcNow,
            Status = (Status)participantDto.Status
        };
        
        await _unitOfWork.ParticipantRepository.AddAsync(participant);
        await _unitOfWork.SaveAsync();
        return FromModel(participant);
    }

    public ParticipantDto FromModel(Participant participant)
    {
        return new()
        {
            Room = new RoomDto
            {
                Name = participant.Room.Name,
                Owner = new UserDto
                {
                    Email = participant.Room.Owner.Email,
                    NickName = participant.Room.Owner.NickName
                },
                Participators = participant.Room.Participators.Select(p => new ParticipantDto
                {
                    Id = p.Id,
                    NickName = p.NickName,
                    StatusDate = p.StatusDate,
                    Status = (StatusDto)p.Status
                }),
                CreatedAt = participant.Room.CreatedAt,
                ExpireAt = participant.Room.ExpireAt,
            },
            Id = participant.Id,
            NickName = participant.NickName,
            Status = StatusDto.Idling,
            StatusDate = DateTime.UtcNow
        };
    }
}