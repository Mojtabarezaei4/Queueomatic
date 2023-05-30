using Microsoft.AspNetCore.Components;
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
}