using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BlazorBootstrapToasts;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.Login;

public partial class LoginComponent : ComponentBase
{
    private LoginDto _loginDto = new();
    private bool _isClicked = false;
    private string _buttonContent = "Login";
    private string _responseMessage = String.Empty;
    private Toast Toast { get; set; }

    private async Task Login()
    {
        _isClicked = true;
        _buttonContent = "Processing...";
        var loginRequest = new LoginRequest(new()
        {
            Email = _loginDto.Email.ToLower(),
            Password = _loginDto.Password
        });
        
        var response = await HttpClient.PostAsJsonAsync("api/login", loginRequest);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized || 
            response.StatusCode == HttpStatusCode.BadRequest)
        {
            _responseMessage = "Password or Email is wrong.";
            _loginDto.Email = string.Empty;
            _loginDto.Password = string.Empty;
            _isClicked = false;
            Toast.Show("danger", _responseMessage, 5000);
            _buttonContent = "Login";
            return;
        }
        
        if (response.StatusCode != HttpStatusCode.Unauthorized && 
            response.StatusCode != HttpStatusCode.OK)
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
