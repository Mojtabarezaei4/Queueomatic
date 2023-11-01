﻿using BlazorBootstrapToasts;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Queueomatic.Client.Components.Participant;
using Queueomatic.Shared.DTOs;
using Queueomatic.Shared.Models;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Queueomatic.Client.Pages;

public partial class Room : ComponentBase
{
    [Parameter]
    public string RoomId { get; set; }

    [Parameter]
    public string ParticipantName { get; set; }

    private HubConnection? hubConnection;

    private ParticipantRoomDto _participantRoomDto;
    private RoomDto _roomDto;

    public string RoomName { get; set; } = "TestRoom"; //TODO: verify this

    private List<ParticipantRoomDto> IdlingParticipants = new();
    private List<ParticipantRoomDto> WaitingParticipants = new();
    private List<ParticipantRoomDto> ActiveParticipants = new();
    private AuthenticationState authenticationState;

    protected override async Task OnInitializedAsync()
    {
        InitializeHub();

        authenticationState = await authProvider.GetAuthenticationStateAsync();

        await hubConnection!.StartAsync();
        
        if (authenticationState.User.HasClaim(c => c.Type.Equals("ParticipantId")) || !await IsUserOwner() && !authenticationState.User.IsInRole("Administrator"))
            await InitializeParticipant();
        else
        {
            await hubConnection.InvokeAsync("JoinRoom", RoomId);
            var room = await hubConnection.InvokeAsync<RoomModel?>("GetState", RoomId);
            if (room != null)
                UpdateRoom(room);
        }
    }

    private async Task<bool> IsUserOwner()
    {
        var response = await HttpClient.GetAsync($"api/rooms/{RoomId}");
        var roomResponse = await response.Content.ReadFromJsonAsync<RoomResponse>();
        if (roomResponse == null)
            Navigation.NavigateTo("/error");
        _roomDto = roomResponse.Room!;

        var claimValue = GetClaim("UserId");
        var res = claimValue != null && claimValue.Value.Equals(roomResponse.Room!.Owner.Email);
        return res;
    }

    private void InitializeHub()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri($"/roomHubs/{RoomId}"))
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
        hubConnection.On<RoomModel>("UpdateRoom", (room) =>
        {
            UpdateRoom(room);
            StateHasChanged();
        });
    }

    private async Task InitializeParticipant()
    {
        if (!await SessionStorageService.ContainKeyAsync("authToken"))
            await SessionStorageService.SetItemAsync("authToken", GetToken());
        authenticationState = await authProvider.GetAuthenticationStateAsync();

        var participantId = Guid.Parse(GetClaim("ParticipantId").Value);


        _participantRoomDto = new ParticipantRoomDto { Id = participantId, NickName = ParticipantName };

        await hubConnection.InvokeAsync("JoinRoom", RoomId);
        await InitializeRoom(_participantRoomDto);
    }

    private Claim? GetClaim(string identifier) => authenticationState.User.Claims.FirstOrDefault(c => c.Type.Equals(identifier));

    private async Task<string> GetToken()
    {
        var addParticipantRequest = new AddParticipantRequest(new ParticipantDto { NickName = ParticipantName });

        var response = await HttpClient.PostAsJsonAsync($"api/rooms/{RoomId}/newParticipant", addParticipantRequest);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            Navigation.NavigateTo("/error");
        }

        var result = await response.Content.ReadFromJsonAsync<PostResult>();
        return result.token;
    }

    private bool CanMoveAsync(ParticipantRoomDto participant)
    {
        var userClaim = GetClaim("UserId");
        var participantClaim = GetClaim("ParticipantId");
        
        var canParticipantMove = participantClaim != null && participant.Id.Equals(Guid.Parse(participantClaim!.Value));
        var isUserOwner = userClaim != null && _roomDto.Owner.Email.Equals(userClaim.Value);
        return canParticipantMove 
               || isUserOwner
               || authenticationState.User.IsInRole("Administrator");
    }

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
            var roomParticipant = room.IdlingParticipants.Where(p => p.Id.Equals(participant.Id)).Concat(
                room.WaitingParticipants.Where(p => p.Id.Equals(participant.Id)).Concat(
                    room.ActiveParticipants.Where(p => p.Id.Equals(participant.Id))))
                .FirstOrDefault();
            if (roomParticipant != null)
            {
                _participantRoomDto.Status = roomParticipant.Status;
            }

            UpdateRoom(room);
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
        //await SessionStorageService.RemoveItemAsync("authToken");
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