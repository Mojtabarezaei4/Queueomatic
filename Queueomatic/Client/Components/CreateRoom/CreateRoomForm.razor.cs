using BlazorBootstrapToasts;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace Queueomatic.Client.Components.CreateRoom;

public partial class CreateRoomForm : ComponentBase
{
    private CreateRoomRequest _roomDto = new();

    private Toast Toast { get; set; }
    private async Task CreateRoom()
    {
        var response = await HttpClient.PostAsJsonAsync($"api/room/create", _roomDto);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            Toast.Show("warning", "Something went wrong", 5000);
        }

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PostResult>();

            await UpdateVisitedRooms.UpdateLocalStorage(result!.RoomId);
            NavigationManager.NavigateTo($"rooms/{result!.RoomId}");
        }
    }

    private async Task ShowInfo()
    {
        await Toast.Show("warning", "If you don't provide a name, some random name will be assigned.", 5000);
    }
}

class CreateRoomRequest
{
    public string Name { get; set; } = String.Empty;
}

record PostResult(string Message, string RoomId);