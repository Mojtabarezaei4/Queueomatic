using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;

namespace Queueomatic.DataAccess.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationContext _context;

    public RoomRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetAsync(int id)
    {
        return await _context.Rooms
            .Include(r => r.Owner)
            .Include(r => r.Participators)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _context.Rooms
            .Include(r => r.Owner)
            .Include(r => r.Participators)
            .ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetAllAsync(int id, string name)
    {
        return await _context.Rooms
            .Include(r => r.Owner)
            .Include(r => r.Participators)
            .Where(r => id < 0 && r.Name.Contains(name) || r.Id == id || r.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetExpiredRoomsAsync()
    {
        return await _context.Rooms
            .Where(r => r.ExpireAt <= DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task AddAsync(Room entity)
    {
        await _context.Rooms.AddAsync(entity);
    }

    public Task UpdateAsync(Room entity)
    {
        _context.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var room = await GetAsync(id);
        if (room != null)
        {
            _context.Rooms.Remove(room);
        }
    }
    
    public Task Delete(Room entity)
    {
        _context.Rooms.Remove(entity);
        return Task.CompletedTask;
    }

    
}