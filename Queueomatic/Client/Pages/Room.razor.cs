using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Pages;

public partial class Room : ComponentBase
{
    [Parameter]
    public string RoomId { get; set; }

    [Parameter]
    public string ParticipantName { get; set; }

    public string RoomName { get; set; } = "TestRoom";

    public List<string> IdlingParticipants = new() { "Erik", "Jan" };
    public List<string> WaitingParticipants = new() { "John", "Eliza" };
    public List<string> ActiveParticipants = new() { "Keller", "Rebecca" };

    private bool CanMove(string user) => user.Equals(ParticipantName); //should be updated with participant Id


    private void UpdateUser(string user, StatusDto status)
    {
        //Update SignalR Clients
    }

    private HubConnection? hubConnection;
    private List<string> messages = new List<string>();
    private string? userInput;
    private string? messageInput;

    protected override async Task OnInitializedAsync()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri($"/rooms/{RoomId}"))
            .Build();

        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            var encodedMsg = $"{user}: {message}";
            messages.Add(encodedMsg);
            StateHasChanged();
        });

        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("JoinRoom", RoomId);

    }

    private async Task Send()
    {
        if (hubConnection is not null)
        {
            await hubConnection.SendAsync("SendMessage", userInput, messageInput, RoomId);
        }
    }

    public bool IsConnected =>
        hubConnection?.State == HubConnectionState.Connected;

   
}