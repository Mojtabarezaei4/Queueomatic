using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Queueomatic.Shared.DTOs;

namespace Queueomatic.Client.Components.SignUp;

public partial class SignUpComponent : ComponentBase
{
    private SignupDto _signUpDto = new();
    private bool isClicked = false;
    private async Task SignUp()
    {
        isClicked = true;
        var signUpRequest = new SignUpRequest(new()
        {
            NickName = _signUpDto.NickName,
            Email = _signUpDto.Email.ToLower(),
            Password = _signUpDto.Password,
            ConfirmPassword = _signUpDto.ConfirmPassword
        });

        var response = await HttpClient.PostAsJsonAsync("api/signup", signUpRequest);

        if (!response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/error");
            return;
        }

        NavigationManager.NavigateTo("/login");
    }
}

record SignUpRequest(SignupDto Signup);