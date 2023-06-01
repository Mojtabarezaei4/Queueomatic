using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.Profile;

public partial class ProfileComponent : ComponentBase
{
    private UserDto User { get; set; } = new();

    private bool _modalIsOpen = false;

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

        User = response.user;
    }

    private void OpenModal()
    {
        _modalIsOpen = true;
    }
    
    private void CloseModal()
    {
        _modalIsOpen = false;
    }
}

record ResultFromGet(UserDto user); 
record ClaimsSchema(string token, string userName);