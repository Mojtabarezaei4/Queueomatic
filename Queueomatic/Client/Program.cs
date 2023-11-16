using Blazored.Modal;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Plk.Blazor.DragDrop;
using Queueomatic.Client;
using Queueomatic.Shared.DTOs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredModal();

builder.Services.AddAuthorizationCore(options => 
    options.AddPolicy("IsOwnerOrAdministrator", policyBuilder =>
    {
        policyBuilder.RequireAssertion(context =>
        {
            if (context.User.IsInRole("Administrator"))
                return true;

            if (context.Resource is not RoomDto room) return false;
            var userIdClaim = context.User.Claims.FirstOrDefault(x => x.Type.Equals("UserId"));
            return userIdClaim != null && userIdClaim.Value.Equals(room.Owner.Email);
        });
     }));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddBlazorDragDrop();

await builder.Build().RunAsync();