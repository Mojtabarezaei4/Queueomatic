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

        var visitedRooms = await _localStorageService.GetItemAsync<SortedDictionary<DateTime, string>>(localStorageName);

        if (visitedRooms is null ||
            !visitedRooms.Any())
        {
            visitedRooms = new SortedDictionary<DateTime, string>
            {
                { DateTime.UtcNow , visitingRoom }
            };

            await _localStorageService.SetItemAsync(localStorageName, visitedRooms);

            return;
        }

        if (visitedRooms.ContainsValue(visitingRoom))
        {
            visitedRooms.Remove(
                visitedRooms
                    .FirstOrDefault(pair => string.Equals(pair.Value, visitingRoom))
                    .Key
            );
        }

        visitedRooms.Add(DateTime.UtcNow, visitingRoom);

        var top = visitedRooms.FirstOrDefault();

        if (top.Value is not null && visitedRooms.Count > 3)
        {
            visitedRooms.Remove(top.Key);
        }

        await _localStorageService.SetItemAsync(localStorageName, visitedRooms);
    }
}