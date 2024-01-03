using Microsoft.AspNetCore.Components;
using Queueomatic.Client.Services.StoreVisitedRooms;
using Queueomatic.Shared.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace Queueomatic.Client.Components.VisitedRooms;

public partial class VisitedRoomsComponent : ComponentBase
{
    private List<RoomDto> _visitedRooms = new();

    const string LocalStorageName = "visitedRooms";

    protected override async Task OnInitializedAsync()
    {
        var visitedRooms = await LocalStorageService.GetItemAsync<SortedDictionary<DateTime, string>>(LocalStorageName);
        
        if (visitedRooms is not null)
        {
            foreach (var visitedRoom in visitedRooms)
            {
                var response = await HttpClient.GetAsync($"api/rooms/{visitedRoom.Value}");
                
                if (response.StatusCode != HttpStatusCode.OK) continue;

                var room = await response.Content.ReadFromJsonAsync<RoomResponse>();

                _visitedRooms.Add(room!.Room);
            }
        }
    }
    private async Task GotoRoom(string roomHashId)
    {
        var response = await HttpClient.GetAsync($"api/rooms/{roomHashId}");

        if (!response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/error");
            return;
        }

        await UpdateVisitedRooms.UpdateLocalStorage(roomHashId);

        NavigationManager.NavigateTo($"/rooms/{roomHashId}");
    }
}
record RoomResponse(RoomDto Room);