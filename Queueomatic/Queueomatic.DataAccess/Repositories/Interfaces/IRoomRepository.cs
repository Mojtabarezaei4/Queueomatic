using Queueomatic.DataAccess.Models;

namespace Queueomatic.DataAccess.Repositories.Interfaces;

public interface IRoomRepository : IRepository<Room, int> 
{
    Task<IEnumerable<Room>> GetAllAsync(int id, string name);
    Task<IEnumerable<Room>> GetExpiredRoomsAsync();
    Task DeleteAsync(Room entity);
    Task<Room?> GetAsync(int id);
}