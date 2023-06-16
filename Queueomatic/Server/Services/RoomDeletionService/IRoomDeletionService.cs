namespace Queueomatic.Server.Services.RoomDeletionService;

public interface IRoomDeletionService
{
    Task DeleteExpiredRoomsAsync();
}