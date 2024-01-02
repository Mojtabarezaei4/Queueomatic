using System.Net.Http.Json;
using BlazorBootstrapToasts;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.EditProfile;

public partial class EditProfileComponent : ComponentBase
{

    [Parameter] 
    public UserDto User { get; set; }

    [Parameter]
    public Action CloseModalEvent { get; set; }
    private Toast Toast { get; set; }

    private async Task Update()
    {
        var result = await HttpClient.PostAsJsonAsync("/user/username", User);
        if (!result.IsSuccessStatusCode)
        {
            Toast.Show("warning", "Something went wrong. Try again later.", 5000);
        }
        else
        {
            await Toast.Show("success", "Username successfully changed!", 3000);
            CloseModalEvent.Invoke();
        }
    }
}