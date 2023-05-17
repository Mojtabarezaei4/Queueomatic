using Queueomatic.DataAccess.Models;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Server.Services.CreateRoomService;

public interface ICreateRoomService
{
    public Task<bool> CreateRoomAsync(RoomDto room, string userEmail);
}