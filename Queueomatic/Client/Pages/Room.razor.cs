using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Queueomatic.Client.Components.Participant;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Models;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Queueomatic.Client.Pages;

public partial class Room : ComponentBase
{
    [Parameter]
    public string RoomId { get; set; } = string.Empty;

    public bool IsOwner { get; set; }

    [Parameter]
    public string ParticipantName { get; set; } = string.Empty;

    private HubConnection? _hubConnection;

    private ParticipantRoomDto? _participantRoomDto;
    private RoomDto? _roomDto;

    [Parameter]
    [SupplyParameterFromQuery(Name = "room-name")]
    public string RoomName { get; set; } = string.Empty;

    private List<ParticipantRoomDto> _idlingParticipants = new();
    private List<ParticipantRoomDto> _waitingParticipants = new();
    private List<ParticipantRoomDto> _activeParticipants = new();
    private AuthenticationState? _authenticationState;

    protected override async Task OnInitializedAsync()
    {
        _authenticationState = await AuthProvider.GetAuthenticationStateAsync();

        if (!_authenticationState.User.HasClaim(c => c.Type.Equals("ParticipantId")) &&
            (await IsUserOwner() || _authenticationState.User.IsInRole("Administrator")))
        {
            InitializeHub(await SessionStorageService.GetItemAsync<string>("authToken"));
            await _hubConnection!.StartAsync();

            await _hubConnection.InvokeAsync("JoinRoom", RoomId);
            var room = await _hubConnection.InvokeAsync<RoomModel?>("GetState", RoomId);
            if (room != null)
                UpdateRoom(room);

            IsOwner = true;
        }
        else
        {
            InitializeHub("");
            await _hubConnection!.StartAsync();
            await InitializeParticipant();
        }
    }

    private async Task<bool> IsUserOwner()
    {
        var response = await HttpClient.GetAsync($"api/rooms/{RoomId}");
        var roomResponse = await response.Content.ReadFromJsonAsync<RoomResponse>();
        if (roomResponse == null)
            Navigation.NavigateTo("/error");
        _roomDto = roomResponse!.Room;
        RoomName = _roomDto.Name;

        var claimValue = GetClaim("UserId");
        return claimValue != null && claimValue.Value.Equals(roomResponse.Room!.Owner.Email);
    }

    private void InitializeHub(string token)
    {
        var queryParam = "";
        if (!string.IsNullOrWhiteSpace(token))
            queryParam = $"?authToken={token}";
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri($"/roomHubs/{RoomId}{queryParam}"))
            .Build();

        _hubConnection.On<ParticipantRoomDto, StatusDto>("MoveParticipant", (user, status) =>
        {
            MoveParticipant(user, status);
            StateHasChanged();
        });

        _hubConnection.On<ParticipantRoomDto>("ClearRoom", (user) =>
        {
            ClearTheRoom(user);
            StateHasChanged();
        });
        _hubConnection.On("KickParticipant", async () =>
        {
            await SessionStorageService.RemoveItemAsync("authToken");
            Navigation.NavigateTo("/");
        });
    }

    private async Task InitializeParticipant()
    {
        if (!await SessionStorageService.ContainKeyAsync("authToken"))
            await SessionStorageService.SetItemAsync("authToken", GetToken());
        _authenticationState = await AuthProvider.GetAuthenticationStateAsync();

        var participantId = Guid.Parse(GetClaim("ParticipantId")!.Value);
        

        _participantRoomDto = new ParticipantRoomDto { Id = participantId, NickName = ParticipantName, ConnectionId = _hubConnection!.ConnectionId! };

        await _hubConnection.InvokeAsync("JoinRoom", RoomId);
        await InitializeRoom(_participantRoomDto);
    }

    private Claim? GetClaim(string identifier) => _authenticationState!.User.Claims.FirstOrDefault(c => c.Type.Equals(identifier));

    private async Task<string> GetToken()
    {
        var addParticipantRequest = new AddParticipantRequest(new ParticipantDto { NickName = ParticipantName });

        var response = await HttpClient.PostAsJsonAsync($"api/rooms/{RoomId}/newParticipant", addParticipantRequest);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            Navigation.NavigateTo("/error");
        }

        var result = await response.Content.ReadFromJsonAsync<PostResult>();
        return result!.Token;
    }

    private bool CanMoveAsync(ParticipantRoomDto participant)
    {
        var userClaim = GetClaim("UserId");
        var participantClaim = GetClaim("ParticipantId");

        var canParticipantMove = participantClaim != null && participant.Id.Equals(Guid.Parse(participantClaim!.Value));
        var isUserOwner = userClaim != null && _roomDto!.Owner.Email.Equals(userClaim.Value);
        return canParticipantMove
               || isUserOwner
               || _authenticationState!.User.IsInRole("Administrator");
    }

    private async Task UpdateUser(ParticipantRoomDto participant, StatusDto status)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.SendAsync("UpdateParticipant", participant, status, RoomId);
        }
    }

    private async Task InitializeRoom(ParticipantRoomDto participant)
    {
        if (_hubConnection is not null)
        {
            var room = await _hubConnection.InvokeAsync<RoomModel>("InitializeParticipant", participant, RoomId, RoomName);
            var roomParticipant = room.IdlingParticipants.Where(p => p.Id.Equals(participant.Id)).Concat(
                room.WaitingParticipants.Where(p => p.Id.Equals(participant.Id)).Concat(
                    room.ActiveParticipants.Where(p => p.Id.Equals(participant.Id))))
                .FirstOrDefault();
            if (roomParticipant != null)
            {
                _participantRoomDto!.Status = roomParticipant.Status;
            }

            UpdateRoom(room);
        }
    }

    private void UpdateRoom(RoomModel room)
    {
        _idlingParticipants = room.IdlingParticipants;
        _waitingParticipants = room.WaitingParticipants;
        _activeParticipants = room.ActiveParticipants;

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
        activeList.Remove(participantToBeDeleted!);
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
            StatusDto.Idling => _idlingParticipants,
            StatusDto.Waiting => _waitingParticipants,
            StatusDto.Ongoing => _activeParticipants,
            _ => throw new ArgumentOutOfRangeException(nameof(participantStatus), participantStatus, null)
        };
    }


    public bool IsConnected =>
        _hubConnection?.State == HubConnectionState.Connected;


    private async Task Exit()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.SendAsync("LeaveRoom", _participantRoomDto, RoomId, "");
        }
        Navigation.NavigateTo("/");
    }

    private void ClearTheRoom(ParticipantRoomDto participantRoomDto)
    {
        if (_activeParticipants.Any(p => p.Id.Equals(participantRoomDto.Id)))
        {
            RemoveOldUser(participantRoomDto, GetList(StatusDto.Ongoing));
        }

        if (_idlingParticipants.Any(p => p.Id.Equals(participantRoomDto.Id)))
        {
            RemoveOldUser(participantRoomDto, GetList(StatusDto.Idling));
        }

        if (_waitingParticipants.Any(p => p.Id.Equals(participantRoomDto.Id)))
        {
            RemoveOldUser(participantRoomDto, GetList(StatusDto.Waiting));
        }
    }

    private async Task KickUser(string connectionIdOfParticipant, Guid participantId)
    {
        if (_hubConnection is not null)
            await _hubConnection.InvokeAsync("KickParticipant", RoomId, connectionIdOfParticipant, participantId);
    }
}