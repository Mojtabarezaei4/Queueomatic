using Microsoft.EntityFrameworkCore;

namespace Queueomatic.DataAccess.DataContexts;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {

    }
}