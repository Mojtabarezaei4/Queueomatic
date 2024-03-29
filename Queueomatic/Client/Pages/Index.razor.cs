﻿using System.Net;
using System.Net.Http.Json;
using BlazorBootstrapToasts;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Queueomatic.Client.Components.Participant;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Pages;

public partial class Index : ComponentBase
{
    private bool _isClicked = false;
    private string _buttonContent = "Join";
    [CascadingParameter]
    IModalService? SignupModal { get; set; }
    private string _roomId = string.Empty;
    private Toast? Toast { get; set; }
    private async Task CheckRoomIsValid()
    {
        _isClicked = true;
        _buttonContent = "Processing...";

        if (_roomId.Length < 6)
        {
            _ = Toast!.Show("warning", $"Minimum length of room id is 6 characters!", 5000);
            _isClicked = false;
            _buttonContent = "Join";
            return;
        }

        var response = await HttpClient.GetAsync($"api/rooms/{_roomId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _ = Toast!.Show("warning", "No room with provided id was found!", 5000);
            _isClicked = false;
            _buttonContent = "Join";
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var room = await response.Content.ReadFromJsonAsync<RoomResponse>();

            var parameters = new ModalParameters()
                .Add(nameof(ParticipantSignupForm.RoomName), room!.Room.Name)
                .Add(nameof(ParticipantSignupForm.RoomId), room!.Room.HashId);
            SignupModal!.Show<ParticipantSignupForm>("", parameters);

            _isClicked = false;
            _buttonContent = "Join";
        }
    }
}

record RoomResponse(RoomDto Room);