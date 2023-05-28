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
        
        var response = await _httpClient.PostAsJsonAsync("api/login", loginRequest);

        if (!response.IsSuccessStatusCode)
        {        
            _navigationManager.NavigateTo("/error");
            return;
        }
        
        var responseContent = await response.Content.ReadFromJsonAsync<PostResult>();
        await _sessionStorageService.SetItemAsync("authToken", responseContent!);
        _navigationManager.NavigateTo("/");
    }
}

record LoginRequest(LoginDto Login);
record PostResult(string token, string userName);
