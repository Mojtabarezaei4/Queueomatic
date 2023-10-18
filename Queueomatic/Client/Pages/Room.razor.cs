using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Models;

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
    private List<ParticipantRoomDto> ActiveParticipants = new();

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

        hubConnection.On<ParticipantRoomDto>("ClearTheRoom", (user) =>
        {
            ClearTheRoom(user);
            StateHasChanged();
        });


        var participantId = Guid.NewGuid();

        if (await SessionStorageService.ContainKeyAsync("clientId"))
        {
            participantId = await SessionStorageService.GetItemAsync<Guid>("clientId");

        }
        else
            await SessionStorageService.SetItemAsync("clientId", participantId);

        _participantRoomDto = new ParticipantRoomDto { Id = participantId, NickName = ParticipantName };

        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("JoinRoom", RoomId);
        await InitializeRoom(_participantRoomDto);
    }


    private bool CanMove(ParticipantRoomDto participant) => participant.NickName.Equals(ParticipantName); //should be updated with participant Id

    private async Task UpdateUser(ParticipantRoomDto participant, StatusDto status)
    {
        if (hubConnection is not null)
        {
            await hubConnection.SendAsync("UpdateParticipant", participant, status, RoomId);
        }
    }

    private async Task InitializeRoom(ParticipantRoomDto participant)
    {
        if (hubConnection is not null)
        {
            var room = await hubConnection.InvokeAsync<RoomModel>("InitializeParticipant", participant, RoomId, RoomName);

            IdlingParticipants = room.IdlingParticipants;
            WaitingParticipants = room.WaitingParticipants;
            ActiveParticipants = room.ActiveParticipants;

            StateHasChanged();
        }
    }

    private void UpdateRoom(RoomModel room)
    {
        IdlingParticipants = room.IdlingParticipants;
        WaitingParticipants = room.WaitingParticipants;
        ActiveParticipants = room.ActiveParticipants;

        StateHasChanged();
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
            await hubConnection.SendAsync("LeaveRoom", _participantRoomDto, RoomId);
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