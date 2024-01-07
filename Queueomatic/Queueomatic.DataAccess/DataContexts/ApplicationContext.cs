using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Queueomatic.DataAccess.Models;

namespace Queueomatic.DataAccess.DataContexts;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        try
        {
            if (Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator databaseCreator) return;
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}