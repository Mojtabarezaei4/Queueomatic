using Queueomatic.DataAccess.Models;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.ParticipantService;

public interface IParticipantService
{
    Task<ParticipantDto?> CreateOneAsync(ParticipantDto participantDto, string roomId);
    Task<bool> UpdateOneAsync(ParticipantDto participantDto, Guid id);
    public ParticipantDto FromModel(Participant participant);
}