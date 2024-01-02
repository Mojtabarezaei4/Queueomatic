using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Queueomatic.Client.Components.EditProfile;
using Queueomatic.Client.Components.Participant;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.Profile;

public partial class ProfileComponent : ComponentBase
{
    private UserDto _user = new();
    private UserDto _updatedUserInfo = new();
    private IModalReference? _modalReference;

    [CascadingParameter] 
    IModalService EditProfileModal { get; set; }

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
            .Add(nameof(EditProfileComponent.CloseModalEvent), new Action(CloseModal));
        _modalReference = EditProfileModal.Show<EditProfileComponent>(null, parameters);
    }

    private void CloseModal() =>
        _modalReference?.Close();
}

record ResultFromGet(UserDto user); 
record ClaimsSchema(string token, string userName);