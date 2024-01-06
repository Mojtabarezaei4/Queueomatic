using System.Net.Http.Json;
using BlazorBootstrapToasts;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.EditProfile;

public partial class EditProfileComponent : ComponentBase
{

    [Parameter] 
    public UserDto? User { get; set; }

    [Parameter]
    public Action<string>? CloseModalAction { get; set; }
    private Toast? Toast { get; set; }
    private bool _canUpdate;
    private async Task Update()
    {
        _canUpdate = !_canUpdate;
        var result = await HttpClient.PostAsJsonAsync("api/user/username", User!.NickName);
        if (!result.IsSuccessStatusCode)
        {
            _ = Toast!.Show("warning", "Something went wrong. Try again later.", 5000);
            _canUpdate = !_canUpdate;
        }
        else
        {
            await Toast!.Show("success", "Username successfully changed!", 2000);
            CloseModalAction!.Invoke((await result.Content.ReadAsStringAsync()).Trim('\"'));
        }
    }
}