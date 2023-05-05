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

    public async Task<Room?> GetAsync(Guid id)
    {
        return await _context.Rooms.FindAsync(id);
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _context.Rooms.Include(r => r.Owner).Include(r => r.Participators).ToListAsync();
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

    public async Task DeleteAsync(Guid id)
    {
        var room = await GetAsync(id);
        if (room != null)
        {
            _context.Rooms.Remove(room);
        }
    }
}