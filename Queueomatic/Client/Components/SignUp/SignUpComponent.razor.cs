using System.Net;
using System.Net.Http.Json;
using BlazorBootstrapToasts;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.SignUp;

public partial class SignUpComponent : ComponentBase
{
    private SignupDto _signUpDto = new();
    private string _buttonContent = "Signup";
    private bool isClicked = false;
    private string _responseMessage = String.Empty;
    private Toast Toast { get; set; }
    
    private async Task SignUp()
    {
        _buttonContent = "Processing...";
        isClicked = true;
        var signUpRequest = new SignUpRequest(new()
        {
            NickName = _signUpDto.NickName ?? String.Empty,
            Email = _signUpDto.Email.ToLower(),
            Password = _signUpDto.Password,
            ConfirmPassword = _signUpDto.ConfirmPassword
        });

        var response = await HttpClient.PostAsJsonAsync("api/signup", signUpRequest);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            _responseMessage = response.Content
                .ReadAsStringAsync()
                .Result
                .Replace("\"", ""); 
            isClicked = false;
            _buttonContent = "Signup";
            Toast.Show("danger", _responseMessage, 5000);
            return;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/error");
            return;
        }
        NavigationManager.NavigateTo("/login");
    }
}

record SignUpRequest(SignupDto Signup);