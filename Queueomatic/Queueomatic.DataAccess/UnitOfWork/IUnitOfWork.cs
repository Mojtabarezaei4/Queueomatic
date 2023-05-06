using Queueomatic.DataAccess.Repositories.Interfaces;

namespace Queueomatic.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    public IRoomRepository RoomRepository { get; }
    public Task<int> SaveAsync();
    public void Dispose();

}