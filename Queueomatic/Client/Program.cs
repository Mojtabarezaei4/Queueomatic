using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Plk.Blazor.DragDrop;
using Queueomatic.Client;
using Queueomatic.Client.Services.StoreVisitedRooms;
using Queueomatic.Shared.DTOs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredModal();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore(options =>
    {
        options.AddPolicy("IsOwnerOrAdministrator", policyBuilder =>
        {
            policyBuilder.RequireAssertion(context =>
            {
                if (context.Resource is not RoomDto room) return false;

                if (context.User.HasClaim(c => c.Type.Equals("role") && c.Value.Equals("Administrator")))
                    return true;

                var userIdClaim = context.User.Claims.FirstOrDefault(x => x.Type.Equals("UserId"));
                return userIdClaim != null && userIdClaim.Value.Equals(room.Owner.Email);
            });
        });
        options.AddPolicy("SignedInUser", x =>
        {
            x.RequireAssertion(ctx =>
                ctx.User.HasClaim(y => y.Type.Equals("role") && (y.Value.Equals("Administrator") || y.Value.Equals("User"))));
        });
        options.AddPolicy("IsParticipant", x =>
        {
            x.RequireAssertion(ctx =>
            {
                var a = ctx.User.HasClaim(y => y.Type.Equals("role") && y.Value.Equals("Participant"));
                return a;
            });
        });
    }
);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IUpdateVisitedRooms, UpdateVisitedRooms>();
builder.Services.AddBlazorDragDrop();

await builder.Build().RunAsync();