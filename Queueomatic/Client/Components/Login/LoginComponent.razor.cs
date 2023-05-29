using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.Login;

public partial class LoginComponent : ComponentBase
{
    private LoginDto _loginDto = new();

    private async Task Login()
    {
        var loginRequest = new LoginRequest(new()
        {
            Email = _loginDto.Email.ToLower(),
            Password = _loginDto.Password
        });
        
        var response = await HttpClient.PostAsJsonAsync("api/login", loginRequest);

        if (!response.IsSuccessStatusCode)
        {        
            NavigationManager.NavigateTo("/error");
            return;
        }
        
        var responseContent = await response.Content.ReadFromJsonAsync<PostResult>();
        await SessionStorageService.SetItemAsync("authToken", responseContent!);
        NavigationManager.NavigateTo("/");
    }
}

record LoginRequest(LoginDto Login);
record PostResult(string token, string userName);
