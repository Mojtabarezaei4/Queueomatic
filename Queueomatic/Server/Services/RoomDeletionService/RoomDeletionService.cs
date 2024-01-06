using Queueomatic.DataAccess.UnitOfWork;

namespace Queueomatic.Server.Services.RoomDeletionService;

public sealed class RoomDeletionService : IRoomDeletionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RoomDeletionService> _logger;

    public RoomDeletionService(IUnitOfWork unitOfWork, ILogger<RoomDeletionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task DeleteExpiredRoomsAsync()
    {
        var roomsToBeDeleted = await _unitOfWork
            .RoomRepository
            .GetExpiredRoomsAsync();
        foreach (var room in roomsToBeDeleted)
        {
            await _unitOfWork.RoomRepository.DeleteAsync(room);
            
            _logger.LogInformation("Marked Room: {RoomName} with ID: {RoomId} owned by {OwnerEmail} for deletion.",
                room.Name, room.Id ,room.Owner.Email);

            foreach (var participant in room.Participants)
            {
                await _unitOfWork.ParticipantRepository.DeleteAsync(participant.Id);
                _logger.LogInformation("Marked Participant with ID: {ID} with Name: {Nickname} ",
                    participant.Id, participant.NickName);
            }
        }

        await _unitOfWork.SaveAsync();
        _logger.LogInformation("Deleted expired rooms with respective participants.");
    }
}