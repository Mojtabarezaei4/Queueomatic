using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.Models;

namespace Queueomatic.DataAccess.DataContexts;

public class AppContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {

    }
}