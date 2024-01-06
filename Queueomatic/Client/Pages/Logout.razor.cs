using Microsoft.AspNetCore.Components;

namespace Queueomatic.Client.Pages;

public partial class Logout: ComponentBase
{
    private int Count { get; set; } = 5;

    protected override async Task OnInitializedAsync()
    {
        for (int i = 5; i > 0; i--)
        {
            await Task.Delay(1000);
            Count--;
            await InvokeAsync(StateHasChanged);
        }
        await SessionStorageService.RemoveItemAsync("authToken");
        NavigationManager.NavigateTo("/");
    }
}