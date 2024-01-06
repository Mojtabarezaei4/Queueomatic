using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.Models;

namespace Queueomatic.DataAccess.DataContexts;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }
}