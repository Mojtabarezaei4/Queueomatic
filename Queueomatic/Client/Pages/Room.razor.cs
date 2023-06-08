using System.Diagnostics;
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

    private HubConnection? hubConnection;

    public string RoomName { get; set; } = "TestRoom";

    public List<ParticipantRoomDto> IdlingParticipants = new();
    public List<ParticipantRoomDto> WaitingParticipants = new();
    public List<ParticipantRoomDto> ActiveParticipants = new() ;


    private bool CanMove(ParticipantRoomDto participant) => participant.NickName.Equals(ParticipantName); //should be updated with participant Id

    private async Task UpdateUser(ParticipantRoomDto participant, StatusDto status)
    {
        if (hubConnection is not null)
        {
            await hubConnection.SendAsync("UpdateParticipant", participant, status, RoomId);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri($"/rooms/{RoomId}"))
            .Build();

        hubConnection.On<ParticipantRoomDto, StatusDto>("MoveParticipant", (user, status) =>
        {
            MoveParticipant(user, status);
            StateHasChanged();
        });

        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("JoinRoom", RoomId);
        await UpdateUser(new ParticipantRoomDto { Id = Guid.NewGuid(), NickName = ParticipantName}, StatusDto.Idling);
    }

    private void MoveParticipant(ParticipantRoomDto participant, StatusDto status)
    {
        RemoveOldUser(participant, GetList(participant.Status));
        RemoveOldUser(participant, GetList(status));
        AddNewUser(participant, status, GetList(status));
    }

    private void RemoveOldUser(ParticipantRoomDto participant, List<ParticipantRoomDto> activeList)
    {
        var participantToBeDeleted = activeList.FirstOrDefault(x => x.Id == participant.Id);
        activeList.Remove(participantToBeDeleted);
    }

    private void AddNewUser(ParticipantRoomDto participant, StatusDto status, List<ParticipantRoomDto> activeList)
    {
        participant.Status = status;
        activeList.Add(participant);
    }

    private List<ParticipantRoomDto> GetList(StatusDto participantStatus)
    {
        return participantStatus switch
        {
            StatusDto.Idling => IdlingParticipants,
            StatusDto.Waiting => WaitingParticipants,
            StatusDto.Ongoing => ActiveParticipants,
            _ => throw new ArgumentOutOfRangeException(nameof(participantStatus), participantStatus, null)
        };
    }


    public bool IsConnected =>
        hubConnection?.State == HubConnectionState.Connected;

   
}