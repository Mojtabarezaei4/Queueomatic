using System.Net;
using System.Net.Http.Json;
using BlazorBootstrapToasts;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.Participant;

public partial class ParticipantSignupForm: ComponentBase
{
    private ParticipantDto _participantDto = new ();
    
    [Parameter] 
    public string? RoomName { get; set; }
    [Parameter] 
    public string? RoomId { get; set; }
    private Toast? Toast { get; set; }
    private async Task JoinTheRoom()
    {
        var addParticipantRequest = new AddParticipantRequest(_participantDto);
        
        var response = await HttpClient.PostAsJsonAsync($"api/rooms/{RoomId}/newParticipant", addParticipantRequest);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            _ = Toast!.Show("warning", "Something went wrong", 5000);
        }
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<PostResult>();
            await SessionStorageService.SetItemAsync("authToken", result);

            await ManageVisitedRooms.UpdateLocalStorage(RoomId!);

            NavigationManager.NavigateTo($"rooms/{RoomId}/{_participantDto.NickName}?room-name={RoomName}");
        }
    }
}

record AddParticipantRequest(ParticipantDto Participant);
record PostResult(string Token, string ParticipantName);