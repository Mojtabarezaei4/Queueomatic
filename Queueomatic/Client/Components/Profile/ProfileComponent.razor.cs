﻿using System.Net.Http.Json;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Queueomatic.Client.Components.EditProfile;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.Profile;

public partial class ProfileComponent : ComponentBase
{
    private UserDto _user = new();
    private UserDto _updatedUserInfo = new();
    private IModalReference? _modalReference;

    [CascadingParameter]
    IModalService? EditProfileModal { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var userClaims = await SessionStorageService.GetItemAsync<ClaimsSchema>("authToken");

        if (userClaims is null)
        {
            NavigationManager.NavigateTo("/error");
            return;
        }
        
        var response = await HttpClient.GetFromJsonAsync<ResultFromGet>($"api/users/{userClaims.userName}");
    
        if (response is null)
        {
            NavigationManager.NavigateTo("error");
            return;
        }

        _user = response.user;
        _updatedUserInfo = _user;
    }

    private void OpenModal()
    {
        var parameters = new ModalParameters()
            .Add(nameof(EditProfileComponent.User), _user)
            .Add(nameof(EditProfileComponent.CloseModalAction), new Action<string>(CloseModal));
        _modalReference = EditProfileModal!.Show<EditProfileComponent>("", parameters);
    }

    private void CloseModal(string username)
    {
        _modalReference?.Close();
        _user.NickName = username;
        StateHasChanged();
    }

    private async Task JoinRoom(string roomHashId)
    {
        var response = await HttpClient.GetAsync($"api/rooms/{roomHashId}");
        
        if (!response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/error");
            return;
        }

        await ManageVisitedRooms.UpdateLocalStorage(roomHashId);
        
        NavigationManager.NavigateTo($"/rooms/{roomHashId}");
    }
}

record ResultFromGet(UserDto user); 
record ClaimsSchema(string token, string userName);