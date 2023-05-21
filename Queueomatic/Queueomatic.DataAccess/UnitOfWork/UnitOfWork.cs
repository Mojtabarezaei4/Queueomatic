using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.DataAccess.Repositories.Interfaces;

namespace Queueomatic.DataAccess.UnitOfWork;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private bool _disposed;
    private readonly ApplicationContext _context;

    public UnitOfWork(ApplicationContext context, IUserRepository userRepository, IRoomRepository roomRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RoomRepository = roomRepository;
    }
    
    public UnitOfWork(ApplicationContext context, IParticipantRepository participantRepository, IRoomRepository roomRepository)
    {
        _context = context;
        ParticipantRepository = participantRepository;
        RoomRepository = roomRepository;
    }

    public IUserRepository UserRepository { get; }
    public IRoomRepository RoomRepository { get; }
    public IParticipantRepository ParticipantRepository { get; }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}