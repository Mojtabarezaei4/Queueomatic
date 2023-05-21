using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.ParticipantService;

public interface IParticipantService
{
    Task<bool> CreateOneAsync(ParticipantDto participantDto);

    ParticipantDto FromModel(Participant participant);

    Participant ToModel(ParticipantDto participantDto);
}