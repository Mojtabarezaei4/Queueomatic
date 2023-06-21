using BlazorBootstrapToasts;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.EditProfile;

public partial class EditProfileComponent: ComponentBase
{

    [Parameter] 
    public UserDto User { get; set; }
    private Toast Toast { get; set; }

    // TODO: Update function
    private void Update()
    {
        Toast.Show("warning", "This functionality is not working right now.", 5000);
    }
}