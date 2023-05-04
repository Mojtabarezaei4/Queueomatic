namespace Queueomatic.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    public Task<int> SaveAsync();
    public void Dispose();

}