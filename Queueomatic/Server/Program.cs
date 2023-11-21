using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using Queueomatic.DataAccess.DataContexts;
using Queueomatic.Shared.DTOs;
using Queueomatic.DataAccess.Repositories;
using Queueomatic.DataAccess.Repositories.Interfaces;
using Queueomatic.DataAccess.UnitOfWork;
using Queueomatic.Server.Endpoints.Hubs.Room;
using Queueomatic.Server.BackgroundServices;
using Queueomatic.Server.Services.AuthenticationService;
using Queueomatic.Server.Services.HashIdService;
using Queueomatic.Server.Services.MailService;
using Queueomatic.Server.Services.ParticipantService;
using Queueomatic.Server.Services.RoomDeletionService;
using Queueomatic.Server.Services.RoomService;
using Microsoft.Extensions.Caching.Memory;
using Queueomatic.Server.Services.CacheRoomService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Queueomatic.Server.Handlers;
using Queueomatic.Server.Handlers.RoomRestrictionAuthorization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSignalR();
builder.Services.AddFastEndpoints();


var jwtSecret = builder.Configuration.GetSection("JWTSigningKeys").GetSection("DefaultKey").Value;
builder.Services.AddJWTBearerAuth(jwtSecret, bearerEvents: e =>
{
    e.OnMessageReceived = context =>
    {
        var accessToken = context.Request.Query["authToken"];
        var path = context.HttpContext.Request.Path;

        if (!string.IsNullOrEmpty(accessToken.ToString()) && path.StartsWithSegments(new PathString("/roomHubs")))
            context.Token = accessToken.ToString().Replace("\"", "")
                .Split(',')[0].Split(':')[1];
        return Task.CompletedTask;
    };
});



builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IHashIdService, HashIdService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ICacheRoomService, CacheRoomService>();
builder.Services.AddScoped<IAuthorizationHandler, RoomRestrictionRequirementHandler>();
builder.Services.AddTransient(typeof(Random));
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SignedInUser", x => x.RequireRole("User", "Administrator").RequireClaim("UserId"));
    options.AddPolicy("ValidParticipant", x => x.RequireRole("Participant").RequireClaim("ParticipantId"));
    options.AddPolicy("VerifyOwnership", policyBuilder =>
    {
        policyBuilder.Requirements.Add(new RoomRestrictionRequirement());
    });
});

builder.Services.Configure<MailSettingsDto>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddHostedService<ScopedBackgroundService>();
builder.Services.AddScoped<IRoomDeletionService, RoomDeletionService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
});

app.MapHub<RoomHub>("/roomHubs/{id}");
app.MapFallbackToFile("index.html");

app.Run();