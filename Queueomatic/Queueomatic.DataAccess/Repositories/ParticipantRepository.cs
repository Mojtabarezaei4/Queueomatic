using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.Repositories.Interfaces;

namespace Queueomatic.DataAccess.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly ApplicationContext _context;

    public ParticipantRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Participant?> GetAsync(Guid id)
    {
        return await _context.Participants
            .Include(x => x.Room)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Participant>> GetAllAsync()
    {
        return await _context.Participants
            .Include(x => x.Room)
            .ToListAsync();
    }

    public async Task AddAsync(Participant entity)
    {
        await _context.Participants.AddAsync(entity);
    }

    public Task UpdateAsync(Participant entity)
    {
        _context.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var participant = await GetAsync(id);
        if (participant != null)
        {
            _context.Participants.Remove(participant);
        }
    }
}