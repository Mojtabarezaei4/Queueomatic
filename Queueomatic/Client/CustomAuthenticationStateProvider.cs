namespace Queueomatic.Client;

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;


public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ISessionStorageService _sessionStorage;
    private readonly HttpClient _httpClient;

    public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage, HttpClient httpClient)
    {
        _sessionStorage = sessionStorage;
        _httpClient = httpClient;
    }

    //Grab AuthToken from Session Storage then pass claims and create and
    //create a new claims identity then notify the components that wants to be notified,
    //after this the application will know if the user is authenticated or not
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        
        var authToken = await _sessionStorage.GetItemAsync<string>("authToken");

        var identity = new ClaimsIdentity();
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(authToken))
        {
            try
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", "")
                        .Split(',')[0].Split(':')[1]);
            }
            catch
            {
                await _sessionStorage.RemoveItemAsync("authToken");
                identity = new ClaimsIdentity();
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string authToken)
    {
        var payload = authToken.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        var claims = keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string payload)
    {
        switch (payload.Length % 4)
        {
            case 2:
                payload += "==";
                break;
            case 3:
                payload += "=";
                break;
        }

        return Convert.FromBase64String(payload);
    }
}
    
