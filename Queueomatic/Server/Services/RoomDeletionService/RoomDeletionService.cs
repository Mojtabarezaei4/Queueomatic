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
        var roomsToBeDeleted = _unitOfWork.RoomRepository
            .GetAllAsync()
            .Result
            .Where(r => r.ExpireAt <= DateTime.UtcNow)
            .ToList();
        foreach (var room in roomsToBeDeleted)
        {
            await _unitOfWork.RoomRepository.DeleteAsync(room.Id);
            _logger.LogInformation("Deleted Room: {RoomName} owned by {OwnerEmail}",
                room.Name, room.Owner.Email);
            await _unitOfWork.SaveAsync();
        }
    }
}