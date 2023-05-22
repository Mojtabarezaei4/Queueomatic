using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.ParticipantService;

public class ParticipantService : IParticipantService
{
    private readonly IUnitOfWork _unitOfWork;

    public ParticipantService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<bool> CreateOneAsync(ParticipantDto participantDto)
    {
        throw new NotImplementedException();
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
            NickName = participant.NickName,
            Status = StatusDto.Idling,
            StatusDate = DateTime.UtcNow
        };
    }

    public Participant ToModel(ParticipantDto participantDto)
    {
        return new()
        {
            Room = new Room
            {
                Name = participantDto.Room.Name,
                Owner = new User
                {
                    Email = participantDto.Room.Owner.Email,
                    NickName = participantDto.Room.Owner.NickName
                },
                Participators = (ICollection<Participant>)participantDto.Room.Participators.Select(p => new Participant
                {
                    Id = p.Id,
                    NickName = p.NickName,
                    StatusDate = p.StatusDate,
                    Status = (Status)p.Status
                }),
                CreatedAt = participantDto.Room.CreatedAt,
                ExpireAt = participantDto.Room.ExpireAt,
            },
            NickName = participantDto.NickName,
            Status = Status.Idling,
            StatusDate = DateTime.UtcNow
        };
    }
}