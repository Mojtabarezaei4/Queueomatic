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

    private ParticipantRoomDto _participantRoomDto;

    public string RoomName { get; set; } = "TestRoom";

    private List<ParticipantRoomDto> IdlingParticipants = new();
    private List<ParticipantRoomDto> WaitingParticipants = new();
    private List<ParticipantRoomDto> ActiveParticipants = new() ;

    protected override async Task OnInitializedAsync()
    {
        _participantRoomDto = new ParticipantRoomDto { Id = Guid.NewGuid(), NickName = ParticipantName };

        hubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri($"/rooms/{RoomId}"))
            .Build();

        hubConnection.On<ParticipantRoomDto, StatusDto>("MoveParticipant", (user, status) =>
        {
            MoveParticipant(user, status);
            StateHasChanged();
        });
        
        hubConnection.On<ParticipantRoomDto>("ClearTheRoom", (user) =>
        {
            ClearTheRoom(user);
            StateHasChanged();
        });
        
        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("JoinRoom", RoomId);
        await UpdateUser(_participantRoomDto, StatusDto.Idling);
    }
    
    private bool CanMove(ParticipantRoomDto participant) => participant.NickName.Equals(ParticipantName); //should be updated with participant Id

    private async Task UpdateUser(ParticipantRoomDto participant, StatusDto status)
    {
        if (hubConnection is not null)
        {
            await hubConnection.SendAsync("UpdateParticipant", participant, status, RoomId);
        }
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


    private async Task Exit()
    {
        if (hubConnection is not null)
        {
            await hubConnection.SendAsync("LeaveRoom",_participantRoomDto, RoomId);
        }
        await SessionStorageService.RemoveItemAsync("authToken");
        Navigation.NavigateTo("/");
    }

    private void ClearTheRoom(ParticipantRoomDto participantRoomDto)
    {
        if (ActiveParticipants.Any(p => p.Id.Equals(participantRoomDto.Id)))
        {
            RemoveOldUser(participantRoomDto, GetList(StatusDto.Ongoing));
        }
        
        if (IdlingParticipants.Any(p => p.Id.Equals(participantRoomDto.Id)))
        {
            RemoveOldUser(participantRoomDto, GetList(StatusDto.Idling));
        }
        
        if (WaitingParticipants.Any(p => p.Id.Equals(participantRoomDto.Id)))
        {
            RemoveOldUser(participantRoomDto, GetList(StatusDto.Waiting));
        }
    }
}