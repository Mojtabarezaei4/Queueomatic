using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;

namespace Queueomatic.DataAccess.UnitOfWork;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private bool _disposed;
    private readonly ApplicationContext _context;
    //Add repositories below

    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

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