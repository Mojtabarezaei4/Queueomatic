using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Queueomatic.Client.Components.Participant;
using Queueomatic.Shared.DTOs;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Queueomatic.Client.Components.VisitedRooms;

public partial class VisitedRoomsComponent : ComponentBase
{
    [CascadingParameter]
    IModalService? SignupModal { get; set; }
    private List<RoomDto> _visitedRooms = new();
    private AuthenticationState? _authenticationState;

    const string LocalStorageName = "visitedRooms";

    protected override async Task OnInitializedAsync()
    {
        var visitedRooms = await LocalStorageService.GetItemAsync<SortedDictionary<DateTime, string>>(LocalStorageName);

        _authenticationState = await AuthProvider.GetAuthenticationStateAsync();

        if (visitedRooms is null)
        {
            return;
        }

        foreach (var visitedRoom in visitedRooms)
        {
            var response = await HttpClient.GetAsync($"api/rooms/{visitedRoom.Value}");

            if (response.StatusCode != HttpStatusCode.OK) continue;

            var room = await response.Content.ReadFromJsonAsync<RoomResponse>();

            _visitedRooms.Add(room!.Room);
        }
    }

    private async Task JoinRoom(string roomHashId)
    {
        var response = await HttpClient.GetAsync($"api/rooms/{roomHashId}");
        var roomResponse = await response.Content.ReadFromJsonAsync<RoomResponse>();
        
        if (roomResponse is null || !response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/error");
            return;
        }

        if (!_authenticationState!.User.HasClaim(c => c.Type.Equals("ParticipantId")) &&
            (await IsUserOwner(roomHashId) || _authenticationState.User.IsInRole("Administrator"))
            )
        {
            await ManageVisitedRooms.UpdateLocalStorage(roomHashId);

            NavigationManager.NavigateTo($"/rooms/{roomHashId}");
        }

        var parameters = new ModalParameters()
            .Add(nameof(ParticipantSignupForm.RoomName), roomResponse.Room.Name)
            .Add(nameof(ParticipantSignupForm.RoomId), roomResponse.Room.HashId);
        SignupModal!.Show<ParticipantSignupForm>("", parameters);
    }

    private async Task<bool> IsUserOwner(string roomHashId)
    {
        var response = await HttpClient.GetAsync($"api/rooms/{roomHashId}");
        var roomResponse = await response.Content.ReadFromJsonAsync<RoomResponse>();

        if (roomResponse is null)
            return false;

        var claimValue = GetClaim("UserId");
        return claimValue != null && claimValue.Value.Equals(roomResponse.Room.Owner.Email);
    }

    private Claim? GetClaim(string identifier) => _authenticationState!.User.Claims.FirstOrDefault(c => c.Type.Equals(identifier));
}
record RoomResponse(RoomDto Room);