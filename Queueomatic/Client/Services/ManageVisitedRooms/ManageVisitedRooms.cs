using Blazored.LocalStorage;

namespace Queueomatic.Client.Services.ManageVisitedRooms;

public class ManageVisitedRooms : IManageVisitedRooms
{
    private readonly ILocalStorageService _localStorageService;

    public ManageVisitedRooms(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task UpdateLocalStorage(string visitingRoom)
    {
        const string localStorageName = "visitedRooms";
        const int maxVisitedRooms = 3;

        var visitedRooms = await _localStorageService.GetItemAsync<SortedDictionary<DateTime, string>>(localStorageName);

        if (IsNullOrEmpty(visitedRooms))
        {
            visitedRooms = new SortedDictionary<DateTime, string>
            {
                {
                    DateTime.UtcNow, visitingRoom
                }
            };

            await _localStorageService.SetItemAsync(localStorageName, visitedRooms);
            return;
        }

        RemoveVisitedRoom(visitedRooms, visitingRoom);

        visitedRooms.Add(DateTime.UtcNow, visitingRoom);

        RemoveOldestVisitedRoom(visitedRooms, maxVisitedRooms);

        await _localStorageService.SetItemAsync(localStorageName, visitedRooms);
    }

    private bool IsNullOrEmpty(SortedDictionary<DateTime, string>? dictionary)
    {
        return dictionary is null || !dictionary.Any();
    }

    private void RemoveVisitedRoom(SortedDictionary<DateTime, string> visitedRooms, string visitingRoom)
    {
        var pairToRemove = visitedRooms.FirstOrDefault(pair => string.Equals(pair.Value, visitingRoom));

        if (pairToRemove.Key != default)
        {
            visitedRooms.Remove(pairToRemove.Key);
        }
    }

    private void RemoveOldestVisitedRoom(SortedDictionary<DateTime, string> visitedRooms, int maxVisitedRooms)
    {
        if (visitedRooms.Count > maxVisitedRooms)
        {
            visitedRooms.Remove(visitedRooms.First().Key);
        }
    }
}