using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Services.StoreVisitedRooms;

public interface IUpdateVisitedRooms
{
    Task UpdateLocalStorage(string visitingRoom);
}