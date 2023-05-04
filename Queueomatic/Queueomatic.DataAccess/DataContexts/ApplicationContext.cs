using Microsoft.EntityFrameworkCore;

namespace Queueomatic.DataAccess.DataContexts;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }
}