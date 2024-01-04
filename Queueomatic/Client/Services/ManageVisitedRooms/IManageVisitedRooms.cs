namespace Queueomatic.Client.Services.ManageVisitedRooms;

public interface IManageVisitedRooms
{
    Task UpdateLocalStorage(string visitingRoom);
}