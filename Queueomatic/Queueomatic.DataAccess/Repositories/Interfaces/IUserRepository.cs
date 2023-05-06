using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.Models;

namespace Queueomatic.DataAccess.Repositories.Interfaces;

public interface IUserRepository : IRepository<User, string>
{
}